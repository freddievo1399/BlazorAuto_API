using BlazorAuto_API.Abstract;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Syncfusion.Blazor;
using WebApp.Abstract;
using WebApp.Client.Service;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();
SyncfusionLicense.Register();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddScoped(typeof(IExcuteService<>), typeof(ExcuteService<>));
builder.Services.AddScoped<ILoadAssemlyBlazor, LazyMode>();
builder.Services.AddScoped<SwalService>();

await builder.Build().RunAsync();
