using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using RestEase;

namespace WebApp.Client.Service
{
    public class ExcuteService<T>: IExcuteService<T>
    {
        T Value;

        public ExcuteService(HttpClient httpClient)
        {
            Value = RestClient.For<T>(httpClient);
        }

        public V Excute<V>(Func<T, V> func)
        {
            
            return func.Invoke(Value);
        }
    }
}
