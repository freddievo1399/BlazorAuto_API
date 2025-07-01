using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Infrastructure
{
    public class DbConnectionProvider : IDbContext
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public DbConnectionProvider(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public ApplicationDbContext BeginTransaction()
        {
            var _context = _factory.CreateDbContext();
            _context.Database.BeginTransaction();
            return _context;
        }

        public ApplicationDbContext Connection()
        {
            var _context = _factory.CreateDbContext();
            return _context;
        }

        public async Task<PagedResultsOf<T>> GetData<T>(DataRequestDto DataManagerDto, Func<IQueryable<T>, IQueryable<T>>? queryExtend = null) where T : EntityBase
        {
            using (var transaction = Connection())
            {
                var DataManagerRequest = DataManagerDto.ToRequest();
                var query = transaction.Set<T>().AsQueryable();
                if (queryExtend != null)
                {
                    query = queryExtend.Invoke(query);
                }


                if (DataManagerRequest.Where != null && DataManagerRequest.Where.Any())
                    query = DataOperations.PerformFiltering(query, DataManagerRequest.Where, "and");

                if (DataManagerRequest.Search != null && DataManagerRequest.Search.Any())
                    query = DataOperations.PerformSearching(query, DataManagerRequest.Search);

                if (DataManagerRequest.Sorted != null && DataManagerRequest.Sorted.Any())
                    query = DataOperations.PerformSorting(query, DataManagerRequest.Sorted);

                var count = await query.AsNoTracking().CountAsync();

                if (DataManagerRequest.Skip > 0)
                    query = query.Skip(DataManagerRequest.Skip);
                if (DataManagerRequest.Take > 0)
                    query = query.Take(DataManagerRequest.Take);

                var abc = query.ToQueryString();
                var result = await query.AsNoTracking().ToListAsync();
                return PagedResultsOf<T>.Ok(result, count);
            }
        }

    }
}
