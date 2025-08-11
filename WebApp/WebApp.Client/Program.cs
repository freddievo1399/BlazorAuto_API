using BlazorAuto_API.Abstract;
using BlazorAuto_API.BaseUIBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Syncfusion.Blazor;
using WebApp.Abstract;
using WebApp.Client;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();
SyncfusionLicense.Register();
builder.Services.AddScoped(sp => new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddScoped(typeof(IBlazorService<>), typeof(BlazorService<>));
builder.Services.AddScoped<ILoadAssemlyBlazor, LazyMode>();
builder.Services.AddScoped<IAuthentication, AuthenticationForClientService>();
builder.Services.AddScoped<SwalService>();
await builder.Build().RunAsync();
