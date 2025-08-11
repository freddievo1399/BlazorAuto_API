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
        Task<Result> Excute(Func<T, Task<Result>> func);
        Task<ResultOf<V>> Excute<V>(Func<T, Task<ResultOf<V>>> func);
        Task<ResultsOf<V>> Excute<V>(Func<T, Task<ResultsOf<V>>> func);
        Task<PagedResultsOf<V>> Excute<V>(Func<T, Task<PagedResultsOf<V>>> func);
        Task<ClaimsPrincipal> GetUser();
    }
}
