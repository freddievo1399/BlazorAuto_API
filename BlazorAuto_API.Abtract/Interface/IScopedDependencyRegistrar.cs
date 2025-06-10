using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAuto_API.Abstract
{
    public interface IScopedDependencyRegistrar
    {
        public void RegisterServices(IServiceCollection services);
    }
}
