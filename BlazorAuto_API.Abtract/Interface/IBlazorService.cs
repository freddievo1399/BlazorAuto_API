using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public interface IBlazorService<T>
    {
        T GetService();
        V Excute<V>(Func<T, V> func);
        ClaimsPrincipal User { get; }
        void SetUser(string Token);
    }
}
