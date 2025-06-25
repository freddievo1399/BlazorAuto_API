using System;
using System.Reflection;
using System.Reflection.Metadata;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped(typeof(IExcuteService<>), typeof(ExcuteService<>));
builder.Services.RegisterScopedDependency(AssembliesUtil.GetAssemblies().GetInstances<IScopedDependencyRegistrar>());
var abc = builder.Configuration.GetConnectionString("Sqlserver");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("Sqlserver")));

builder.Services.AddScoped<SwalService>();

builder.Logging.AddConsole();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
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
    app.UseSwaggerUI();
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
