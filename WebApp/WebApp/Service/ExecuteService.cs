using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;

namespace WebApp
{
    public class ExecuteService<T> : IExecuteService<T>
    {
        T _apiService { get; set; }
        public ExecuteService(T service)
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
    }
}
