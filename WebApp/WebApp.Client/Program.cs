using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using WebApp.Abstract;
using WebApp.Client.Service;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddScoped<ILoadAssemlyBlazor, LazyMode>();
await builder.Build().RunAsync();
