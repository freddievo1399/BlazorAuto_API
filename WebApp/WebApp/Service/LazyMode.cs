using System.Net.Http.Json;
using System.Reflection;
using BlazorAuto_API.AbstractServer;
using WebApp.Abstract;


namespace WebApp
{
    public class LazyMode : ILoadAssemlyBlazor
    {

        public List<Assembly> LoadAssemblyAsync()
        {
            return AssembliesUtil.GetAssembliesBlazor().ToList();
        }
    }
}
