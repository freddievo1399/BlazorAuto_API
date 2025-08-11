using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Infrastructure
{
    public interface IDbContext
    {
        public Task<ApplicationDbContext> BeginTransactionAsync();
        public ApplicationDbContext Connection();

        public Task<PagedResultsOf<T>> GetData<T>(DataRequestDto DataManagerDto, Func<IQueryable<T>, IQueryable<T>>? queryExtend = null) where T : EntityBase;
    }
}
