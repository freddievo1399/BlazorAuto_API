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
using RestEase;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server;

[ApiController]
[Route("/api/admin/[controller]")]
[ApiExplorerSettings(GroupName = "Admin")]
public class ProductManagerController : ControllerBase, IProductManagerService
{
    IDbContext _DbContext;

    public ProductManagerController(IDbContext DbContext)
    {
        _DbContext = DbContext;
    }
    [HttpPost(nameof(Add))]
    public async Task<Result> Add([FromBody] ProductCreateModel Request)
    {
        try
        {
            using (var db = _DbContext.Connection())
            {
                var abc = new EntityProduct()
                {
                    Name = Request.Name,
                    Description = Request.Description,
                    CreatedBy = "test"
                };
                await db.Set<EntityProduct>().AddAsync(abc);
                await db.SaveChangesAsync();

            }
            return true;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    [HttpGet(nameof(FindByGuid))]
    public async Task<ResultOf<ProductInfoModel>> FindByGuid(Guid Guid)
    {
        try
        {

            using (var db = _DbContext.Connection())
            {
                var entityProduct = await db.Set<EntityProduct>().Include(x => x.Specifications).Include(x => x.ProductCategories).
                    ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Guid == Guid);
                if (entityProduct != null)
                {
                    var res = new ProductInfoModel
                    {
                        Guid = entityProduct.Guid,
                        Name = entityProduct.Name,
                        Description = entityProduct.Description
                    };
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
    public async Task<PagedResultsOf<ProductInfoModel>> GetData([FromBody] DataRequestDto Request)
    {
        try
        {
            var rstTemp = await _DbContext.GetData<EntityProduct>(Request, x => x.Where(x => !x.DeletedAt.HasValue).Include(x => x.ProductCategories).ThenInclude(x => x.Category));
            var rst = PagedResultsOf<ProductInfoModel>.Ok(
                rstTemp.Items.Select(x => new ProductInfoModel
                {
                    Description = x.Description,
                    Guid = x.Guid,
                    Name = x.Name
                }), rstTemp.TotalCount);

            return rst;
        }
        catch (Exception)
        {
            return PagedResultsOf<ProductInfoModel>.Error("An error occurred while fetching data.");
        }
    }
    [HttpPost(nameof(Remove))]
    public async Task<Result> Remove(Guid Guid)
    {
        using (var db = _DbContext.Connection())
        {
            var model = await db.Set<EntityProduct>().FirstOrDefaultAsync(x => x.Guid == Guid);
            if (model == null)
            {
                return "Không tìm thấy";
            }
            model.DeletedBy = "test";
            model.DeletedAt = DateTime.Now;
            db.Set<EntityProduct>().Update(model);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
