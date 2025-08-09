using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;

namespace WebApp
{
    public class BlazorService<T> : IBlazorService<T>
    {
        T _apiService { get; set; }
        static ClaimsPrincipal _user = new();
        public ClaimsPrincipal User { get => _user;}

        public BlazorService(T service)
        {
            _apiService = service;
        }
        public T GetService()
        {
            return _apiService;
        }
        public V Excute<V>(Func<T, V> func)
        {

            return func(_apiService);
        }

        public void SetUser(string Token)
        {
            _user=JwtHelper.CreatePrincipalFromToken(Token);
        }
    }
}
