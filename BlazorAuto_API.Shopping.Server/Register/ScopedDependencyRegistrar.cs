using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.BlazorAuto_API.Shopping.Server;
using BlazorAuto_API.Shopping.Abstract;
using Microsoft.Extensions.DependencyInjection;
namespace BlazorAuto_API.Shopping.Server.Register
{
    public class ScopedDependencyRegistrar : IScopedDependencyRegistrar
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ITest1,Test1Controller>();
        }
    }
}
