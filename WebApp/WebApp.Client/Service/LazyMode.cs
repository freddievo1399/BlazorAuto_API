using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.Logging;
using WebApp.Abstract;
using WebApp.Abtract;

namespace WebApp.Client
{
    public class LazyMode : ILoadAssemlyBlazor
    {
        LazyAssemblyLoader _assemblyLoader;
        HttpClient _httpClient;
        public LazyMode(LazyAssemblyLoader AssemblyLoader, HttpClient HttpClient)
        {
            _assemblyLoader = AssemblyLoader;
            _httpClient = HttpClient;
        }
        public List<Assembly> LoadAllAssembly()
        {
            var temp = AppDomain.CurrentDomain.GetAssemblies();
            return temp.Where(x => x.ManifestModule.Name.EndsWith("Blazor.dll")).ToList();
        }
    }
}
