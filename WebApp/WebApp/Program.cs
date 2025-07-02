using System;
using System.Reflection;
using System.Reflection.Metadata;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.BaseUIBlazor;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Syncfusion.Blazor;
using WebApp;
using WebApp.Abstract;
using WebApp.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSyncfusionBlazor();
SyncfusionLicense.Register();
// Add services to the container.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddHttpClient();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<ILoadAssemlyBlazor, LazyMode>();
builder.Services.AddScoped(typeof(IExecuteService<>), typeof(ExecuteService<>));
builder.Services.RegisterScopedDependency(AssembliesUtil.GetAssemblies().GetInstances<IScopedDependencyRegistrar>());
var abc = builder.Configuration.GetConnectionString("Sqlserver");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("Sqlserver")));

builder.Services.AddScoped<IDbContext, DbConnectionProvider>();
builder.Services.AddScoped<SwalService>();

builder.Logging.AddConsole();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Admin", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Admin API",
        Version = "v1"
    });

    options.SwaggerDoc("Customer", new OpenApiInfo
    {
        Title = "Customer API",
        Version = "v1"
    });

    // Giữ đoạn này nếu bạn có nhiều group
    options.DocInclusionPredicate((groupName, apiDesc) =>
    {
        return apiDesc.GroupName == groupName;
    });
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

//auto migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Apply all pending migrations
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API v1");
        options.SwaggerEndpoint("/swagger/Customer/swagger.json", "Customer API v1");
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

List<Assembly> list = AssembliesUtil.GetAssembliesBlazor().ToList();
list.Add(typeof(WebApp.Client._Imports).Assembly);
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(list.ToArray());

app.Run();
