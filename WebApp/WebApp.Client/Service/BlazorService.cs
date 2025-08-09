using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using RestEase;

namespace WebApp.Client.Service
{
    public class BlazorService<T> : IBlazorService<T>
    {
        readonly T _apiClient;

        static ClaimsPrincipal _user = new();

        public ClaimsPrincipal User { get => _user; }

        public BlazorService(HttpClient httpClient)
        {
            _apiClient = RestClient.For<T>(httpClient);
        }
        public T GetService()
        {
            return _apiClient;
        }
        public V Excute<V>(Func<T, V> func)
        {

            return func.Invoke(_apiClient);
        }

        public void SetUser(string Token)
        {
            _user = JwtHelper.CreatePrincipalFromToken(Token);
        }
    }
}
