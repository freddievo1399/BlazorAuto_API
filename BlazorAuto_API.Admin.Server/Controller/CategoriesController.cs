using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server
{
    [ApiController]
    [Route("/api/admin/[controller]")]


    public class CategoriesController : ControllerBase, ICategoriesService
    {
        IDbContext _DbContext;

        public CategoriesController(IDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        [HttpPost(nameof(Add))]
        public async Task<Result> Add([FromBody] CategoriesCreateModel Request)
        {
            try
            {
                using (var db = _DbContext.Connection())
                {
                    await db.Set<EntityCategory>().AddAsync(new EntityCategory()
                    {
                        Name = Request.Name,
                        Description = Request.Description,
                        CreatedBy = "test"
                    });
                    var i = await db.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpGet(nameof(FindByGuid))]
        public async Task<ResultOf<CategoriesModel>> FindByGuid(Guid Guid)
        {
            try
            {

                using (var db = _DbContext.Connection())
                {
                    var res = await db.Set<EntityCategory>().Include(x => x.ProductCategories).ThenInclude(x => x.Product).Select(x => new CategoriesModel()
                    {
                        Guid = Guid,
                        Name = x.Name,
                        Description = x.Description,
                        ProductNames = x.ProductCategories.Select(x => x.Product.Name).ToList(),
                    }).FirstOrDefaultAsync(x => x.Guid == Guid);
                    if (res != null)
                    {
                        return res;

                    }
                    return "Không tim thấy";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpPost(nameof(GetData))]
        public async Task<PagedResultsOf<CategoriesModel>> GetData([FromBody] DataRequestDto Request)
        {
            try
            {
                var rstTemp = await _DbContext.GetData<EntityCategory>(Request, x => x.Include(x => x.ProductCategories).ThenInclude(x => x.Product));
                var rst = PagedResultsOf<CategoriesModel>.Ok(
                    rstTemp.Items.Select(x => new CategoriesModel
                    {
                        Description = x.Description,
                        Guid = x.Guid,
                        Name = x.Name,
                        ProductNames = x.ProductCategories.Select(pc => pc.Product.Name).ToList()
                    }), rstTemp.TotalCount);
                return rst;
            }
            catch (Exception)
            {
                return PagedResultsOf<CategoriesModel>.Error("An error occurred while fetching data.");
            }



        }

    }
}
