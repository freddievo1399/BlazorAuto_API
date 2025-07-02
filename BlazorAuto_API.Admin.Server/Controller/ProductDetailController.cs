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
[ApiExplorerSettings(GroupName ="Admin")]
public class ProductDetailController : ControllerBase, IProductDetailService
{
    IDbContext _DbContext;

    public ProductDetailController(IDbContext DbContext)
    {
        _DbContext = DbContext;
    }
    [HttpGet(nameof(FindByGuid))]
    public async Task<ResultOf<ProductDetailModel>> FindByGuid(Guid Guid)
    {
        try
        {
            using (var db = _DbContext.Connection())
            {
                var productDetail = await db.Set<EntityProduct>().Include(x => x.Specifications)
                    .Select(x => new ProductDetailModel()
                    {
                        Guid = x.Guid,
                        Description = x.Description,
                        Name = x.Name,
                        RichDescription = x.RichDescription,
                        CategoryGuids = x.Specifications.Select(spec => spec.Guid).ToList(),
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Guid == Guid);
                if (productDetail == null)
                    return "Không tìm thấy";
                return productDetail;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    [HttpGet(nameof(GetListCategory))]
    public async Task<ResultsOf<CategoriesInfoModel>> GetListCategory()
    {
        try
        {
            using (var db = _DbContext.Connection())
            {
                var CategoryItem = await db.Set<EntityCategory>()
                    .AsNoTracking().Select(x => new CategoriesInfoModel()
                    {
                        Description = x.Description,
                        Guid = x.Guid,
                        Name = x.Name
                    }).ToListAsync();
                return CategoryItem;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost(nameof(GetListCategory))]
    public async Task<Result> Update([Body] ProductUpdateModel Request)
    {
        try
        {
            using (var db = await _DbContext.BeginTransactionAsync())
            {
                var product = await db.Set<EntityProduct>()
                    .Include(x => x.ProductCategories)
                    .FirstOrDefaultAsync(x => x.Guid == Request.Guid);
                if (product == null)
                    return "Không tìm thấy sản phẩm";
                product.RichDescription = Request.RichDescription;
                product.Name = Request.Name;
                product.Description = Request.Description;
                product.UpdatedAt = DateTime.Now;

                var currentCategoryGuids = product.ProductCategories.Select(pc => pc.CategoryGuid).ToHashSet();
                var requestCategoryGuids = Request.CategoryGuids.ToHashSet();

                db.RemoveRange(product.ProductCategories.Where(x => !requestCategoryGuids.Contains(x.CategoryGuid)));
                await db.AddRangeAsync(requestCategoryGuids.Except(currentCategoryGuids).Select(x => new EntityProductCategory
                {
                    ProductGuid = product.Guid,
                    CategoryGuid = x
                }));


                db.Update(product);
                await db.CommitAsync();
                await db.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
