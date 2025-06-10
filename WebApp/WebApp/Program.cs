using System.Reflection;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.AbstractServer;
using WebApp;
using WebApp.Abstract;
using WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<ILoadAssemlyBlazor, LazyMode>();
builder.Services.RegisterScopedDependency(AssembliesUtil.GetAssemblies().GetInstances<IScopedDependencyRegistrar>());
builder.Logging.AddConsole();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

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
