using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server
{
    [ApiController]
    [Route("/api/admin/[controller]")]


    public class CategoriesController : ControllerBase, ICategoriesService
    {
        ApplicationDbContext _applicationDbContext;

        public CategoriesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet(nameof(FindByGuid))]
        public Task<ResultOf<CategoriesModel>> FindByGuid(Guid Guid)
        {
            throw new NotImplementedException();
        }
        [HttpPost(nameof(GetData))]
        public async Task<PagedResultsOf<CategoriesModel>> GetData([FromBody] DataManagerRequest input)
        {
            if (_applicationDbContext == null)
            {
                return "Context null";
            }

            IQueryable<EntityCategory> query = _applicationDbContext.Set<EntityCategory>().AsQueryable();

            // 1. Lọc dữ liệu
            if (input.Where != null && input.Where.Any())
                query = DataOperations.PerformFiltering(query, input.Where, "and") ;

            // 2. Tìm kiếm
            if (input.Search != null && input.Search.Any())
                query = DataOperations.PerformSearching(query, input.Search) ;

            // 3. Sắp xếp
            if (input.Sorted != null && input.Sorted.Any())
                query = DataOperations.PerformSorting(query, input.Sorted) ;

            // 4. Đếm số bản ghi (bắt buộc để phân trang)
            var count = await query.CountAsync();

            // 5. Phân trang
            if (input.Skip > 0)
                query = query.Skip(input.Skip);
            if (input.Take > 0)
                query = query.Take(input.Take);

            // 6. Trả kết quả
            var result = await query.Select(x=>new CategoriesModel()).ToListAsync();
            return  PagedResultsOf<CategoriesModel>.Ok(result, count);

        }

    }
}
