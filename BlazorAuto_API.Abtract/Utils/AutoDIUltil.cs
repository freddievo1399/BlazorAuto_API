using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAuto_API.Abstract
{
    public static class AutoDIUltil
    {
        public static void RegisterScopedDependency(this IServiceCollection services, IEnumerable<IScopedDependencyRegistrar> scopedDependencyRegistrars)
        {
            foreach (var registrar in scopedDependencyRegistrars)
            {
                registrar.RegisterServices(services);
            }
        }
    }
}
