using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAuto_API.Infrastructure
{
    public class ScopedDependencyRegistrar : IModuleRegistrar
    {
        public void RegisterPermistion()
        {
        }

        public void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<IAuth, AuthController>();
        }
    }
}
