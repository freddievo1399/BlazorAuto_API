using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using RestEase;

namespace WebApp.Client
{
    public class BlazorService<T>(HttpClient httpClient, IAuthentication authenticationService) : IBlazorService<T>
    {
        readonly T _apiClient = RestClient.For<T>(httpClient);

        public async Task<ClaimsPrincipal> GetUser()
        {
            var result = await authenticationService.GetInfo();
            return result.Item!;
        }

        public async Task<Result> Excute(Func<T, Task<Result>> func)
        {
            return await func.Invoke(_apiClient);
        }

        public async Task<ResultOf<V>> Excute<V>(Func<T, Task<ResultOf<V>>> func)
        {
            return await func.Invoke(_apiClient);
        }

        public async Task<ResultsOf<V>> Excute<V>(Func<T, Task<ResultsOf<V>>> func)
        {
            return await func.Invoke(_apiClient);
        }
        public async Task<PagedResultsOf<V>> Excute<V>(Func<T, Task<PagedResultsOf<V>>> func)
        {
            var hasPermission = await authenticationService.CheckPermistion();
            if (!hasPermission.Success)
            {
                return hasPermission.Message;
            }
            return await func.Invoke(_apiClient);
        }
    }
}
