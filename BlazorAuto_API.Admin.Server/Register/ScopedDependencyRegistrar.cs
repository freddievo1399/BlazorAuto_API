using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAuto_API.Admin.Server
{
    public class ScopedDependencyRegistrar : IScopedDependencyRegistrar
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesController>();

        }
    }
}
