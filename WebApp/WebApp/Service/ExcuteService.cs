using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;

namespace WebApp
{
    public class ExcuteService<T> : IExcuteService<T>
    {
        T _service { get; set; }
        public ExcuteService() { }
        public ExcuteService(T service)
        {
            _service = service;
        }
        public V Excute<V>(Func<T, V> func)
        {
            return func(_service);
        }
    }
}
