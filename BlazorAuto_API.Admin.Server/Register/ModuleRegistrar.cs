using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using static BlazorAuto_API.Admin.Server.CategoriesManagerController;

namespace BlazorAuto_API.Admin.Server
{
    public class ModuleRegistrar : IModuleRegistrar
    {
        
        public void RegisterPermistion()
        {
            GlobalPermistion.Register<CategoriesManagerController.PERMISSION>();
            GlobalPermistion.Register<ProductManagerController.PERMISSION>();
            GlobalPermistion.Register<ProductDetailController.PERMISSION>();
            GlobalPermistion.Register<ManagerUserController.PERMISSION>();
            GlobalPermistion.Register<ManagerRoleController.PERMISSION>();
            GlobalPermistion.Register<AuthenticationController.PERMISSION>();
            
        }

        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICategoriesManagerService, CategoriesManagerController>();
            services.AddScoped<IProductManagerService, ProductManagerController>();
            services.AddScoped<IProductDetailService, ProductDetailController>();
            services.AddScoped<IManagerUser, ManagerUserController>();
            services.AddScoped<IManagerRole, ManagerRoleController>();
            services.AddScoped<IAuthenticationService, AuthenticationController>();

        }
    }
}
