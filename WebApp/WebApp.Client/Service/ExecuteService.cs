using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using RestEase;

namespace WebApp.Client.Service
{
    public class ExecuteService<T> : IExecuteService<T>
    {
        readonly T _apiClient;

        public ExecuteService(HttpClient httpClient)
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
    }
}
