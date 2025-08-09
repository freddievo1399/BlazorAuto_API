using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Server;
using BlazorAuto_API.BaseUIBlazor;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Syncfusion.Blazor;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using WebApp;
using WebApp.Abstract;
using WebApp.Components;
using WebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký Syncfusion
builder.Services.AddSyncfusionBlazor();
SyncfusionLicense.Register();

// HTTP Context
builder.Services.AddHttpContextAccessor();

// Razor Components (Blazor)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// HttpClient scoped cho Blazor
builder.Services.AddScoped<HttpClient>(sp =>
{
    var accessor = sp.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var baseUri = new Uri($"{request?.Scheme}://{request?.Host}/");
    return new HttpClient { BaseAddress = baseUri };
});
#region Các service nội bộ

builder.Services.RegisterScopedDependencyAndPermistion();
builder.Services.AddScoped<ILoadAssemlyBlazor, LazyMode>();
builder.Services.AddScoped(typeof(IBlazorService<>), typeof(BlazorService<>));
builder.Services.AddScoped<SwalService>();
builder.Services.AddScoped<IDbContext, DbConnectionProvider>();
builder.Services.AddScoped<IAuthenticationForServer, IAuthenticationForServerService>();
builder.Services.AddScoped<IAuthentication, AuthenticationForServerService>();

//builder.Services.AddScoped(provider =>
//{
//    var factory = provider.GetRequiredService<IDbContext>();
//    return factory.Connection(); // tạo instance từ factory
//});

#endregion
#region DbContext + Identity

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Sqlserver"));
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped(p =>
    p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext()
);


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
    option.SignIn.RequireConfirmedEmail = false;
});





#endregion


#region JWT Bearer setup 
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
var jwtissuer = builder.Configuration["jwt:issuer"];
var jwtaudience = builder.Configuration["jwt:audience"];




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtissuer,
        ValidateAudience = true,
        ValidAudience = jwtaudience,
        IssuerSigningKey = securityKey
    };
    //options.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        if (context.Request.Cookies.ContainsKey("access_token"))
    //        {
    //            context.Token = context.Request.Cookies["access_token"];
    //        }
    //        return Task.CompletedTask;
    //    }
    //};

});

#endregion


builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
    option.SwaggerDoc("Admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    option.SwaggerDoc("Customer", new OpenApiInfo { Title = "Customer API", Version = "v1" });
    option.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth API", Version = "v1" });

    option.DocInclusionPredicate((groupName, apiDesc) => apiDesc.GroupName == groupName);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Logging
builder.Logging.AddConsole();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    var AuthenticationForServer = scope.ServiceProvider.GetRequiredService<IAuthenticationForServer>();
    await AuthenticationForServer.CheckUserDefaul();
    await AuthenticationForServer.ClearRefeshToken();
}

// Middleware
app.UseStatusCodePages();
app.UseRouting();

// ✅ Authentication trước


app.UseMiddleware<JwtSyncMiddleware>();


app.UseAuthentication();

app.UseAuthorization();


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API v1");
        options.SwaggerEndpoint("/swagger/Customer/swagger.json", "Customer API v1");
        options.SwaggerEndpoint("/swagger/Auth/swagger.json", "Auth API v1");

    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// ✅ Map API controllers trước Blazor

app.MapControllers();

// Blazor
List<Assembly> list = AssembliesUtil.GetAssembliesBlazor().ToList();
list.Add(typeof(WebApp.Client._Imports).Assembly);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(list.ToArray());


app.Run();
