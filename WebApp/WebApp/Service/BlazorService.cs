using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using RestEase;

namespace WebApp
{
    public class BlazorService<T>(T service, IAuthentication authenticationService) : IBlazorService<T>
    {
        public async Task<ClaimsPrincipal> GetUser()
        {
            var result = await authenticationService.GetInfo();
            return result.Item!;
        }

        public async Task<Result> Excute(Func<T, Task<Result>> func)
        {
            var hasPermission = await authenticationService.CheckPermistion();
            if (!hasPermission.Success)
            {
                return hasPermission.Message;
            }
            return await func(service);
        }

        public async Task<ResultOf<V>> Excute<V>(Func<T, Task<ResultOf<V>>> func)
        {
            var hasPermission = await authenticationService.CheckPermistion();
            if (!hasPermission.Success)
            {
                return hasPermission.Message;
            }
            return await func(service);
        }

        public async Task<ResultsOf<V>> Excute<V>(Func<T, Task<ResultsOf<V>>> func)
        {
            var hasPermission = await authenticationService.CheckPermistion();
            if (!hasPermission.Success)
            {
                return hasPermission.Message;
            }
            return await func(service);
        }
        public async Task<PagedResultsOf<V>> Excute<V>(Func<T, Task<PagedResultsOf<V>>> func)
        {
            var hasPermission = await authenticationService.CheckPermistion();
            if (!hasPermission.Success)
            {
                return hasPermission.Message;
            }
            return await func(service);
        }
    }
}
