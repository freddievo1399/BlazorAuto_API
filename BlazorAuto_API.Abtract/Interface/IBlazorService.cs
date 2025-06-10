using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract
{
    public interface IBlazorService
    {
        Task<T> Action<T>(Func<Task<T>> func) ;

    }
}
