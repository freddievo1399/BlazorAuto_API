using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAuto_API.Customer.Server
{
    public class ModuleRegistrar : IModuleRegistrar
    {
        public void RegisterPermistion()
        {
            //GlobalPermistion.Register();
        }

        public void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<ITest1,Test1Controller>();

        }
    }
}
