using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace BlazorAuto_API.Infrastructure
{
    public static class AutoDIUltil
    {
        public static void RegisterScopedDependencyAndPermistion(this IServiceCollection services)
        {
            var scopedDependencyRegistrars = AssembliesUtil.GetAssemblies().GetInstances<IModuleRegistrar>();

            foreach (var registrar in scopedDependencyRegistrars)
            {
                registrar.RegisterServices(services);
                registrar.RegisterPermistion();
            }
        }
    }
}
