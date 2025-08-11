using System.Net.Http.Json;
using System.Reflection;
using BlazorAuto_API.Infrastructure;
using WebApp.Abstract;


namespace WebApp
{
    public class LazyMode : ILoadAssemlyBlazor
    {

        public List<Assembly> LoadAllAssembly()
        {
            return AssembliesUtil.GetAssembliesBlazor().ToList();
        }
    }
}
