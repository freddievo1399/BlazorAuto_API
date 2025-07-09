using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestEase;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Inputs;

namespace BlazorAuto_API.Admin.Server;

[ApiController]
[Route("/api/admin/[controller]")]
[ApiExplorerSettings(GroupName = "Admin")]
public class ProductDetailController : ControllerBase, IProductDetailService
{
    IDbContext _DbContext;
    public string WebRootPath;
    public ProductDetailController(IDbContext DbContext, IWebHostEnvironment env)
    {
        _DbContext = DbContext;
        WebRootPath = env.WebRootPath;
    }
    [HttpGet(nameof(FindByGuid))]
    public async Task<ResultOf<ProductDetailModel>> FindByGuid(Guid Guid)
    {
        try
        {
            using (var db = _DbContext.Connection())
            {
                var productDetail = await db.Set<EntityProduct>().Include(x => x.Specifications).Include(x => x.ProductCategories)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Guid == Guid);
                if (productDetail == null)
                    return "Không tìm thấy";
                var rps = new ProductDetailModel()
                {
                    Guid = productDetail.Guid,
                    Description = productDetail.Description,
                    Name = productDetail.Name,
                    RichDescription = productDetail.RichDescription,
                    CategoryGuids = productDetail.ProductCategories.Select(spec => spec.Guid).ToList(),
                    ProductSpecifications = productDetail.Specifications?.Select(spec => new ProductSpecificationModel()
                    {
                        Guid = spec.Guid,
                        Description = spec.Description,
                        DicountPrice = spec.DicountPrice,
                        IsEnable = spec.IsEnable,
                        Name = spec.Name,
                        Price = spec.Price
                    }).ToList() ?? new(),
                    CarouselImages = productDetail.CarouselImages?.Select(img => new ImageInfoRes()
                    {
                        Guid = img.Guid,
                        FileName = img.FileName,
                        Src = img.src
                    }).ToList() ?? new(),
                    ImageInfos = productDetail.Images?.Select(img => new ImageInfoRes()
                    {
                        Guid = img.Guid,
                        FileName = img.FileName,
                        Src = img.src
                    }).ToList() ?? new(),
                };


                return rps;
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

    [HttpPost(nameof(Update))]
    public async Task<Result> Update([FromBody] ProductUpdateModel Request)
    {
        try
        {
            using (var db = await _DbContext.BeginTransactionAsync())
            {
                var product = await db.Set<EntityProduct>()
                    .Include(x => x.ProductCategories)
                    .Include(x => x.Specifications)
                    .FirstOrDefaultAsync(x => x.Guid == Request.Guid);
                if (product == null)
                    return "Không tìm thấy sản phẩm";
                product.RichDescription = Request.RichDescription;
                product.Name = Request.Name;
                product.Description = Request.Description;
                product.UpdatedAt = DateTime.Now;

                if (Request.ProductSpecifications == null || Request.ProductSpecifications.Count == 0)
                {
                    if (product.Specifications != null || product.Specifications!.Count != 0)
                    {
                        db.RemoveRange(product.Specifications);

                    }
                }
                else
                {
                    var RequestSpecifications = Request.ProductSpecifications.Select(spec => new EntityProductSpecification()
                    {
                        Guid = spec.Guid,
                        Description = spec.Description,
                        DicountPrice = spec.DicountPrice,
                        IsEnable = spec.IsEnable,
                        Name = spec.Name,
                        Price = spec.Price,
                        CreatedAt = DateTime.Now,
                        ProductGuid = product.Guid
                    }).ToList();
                    var removeSpecification = product.Specifications.
                        Where(curr => !RequestSpecifications.Any(update => update.Guid == curr.Guid));
                    var addSpecification = RequestSpecifications.
                        Where(update => !product.Specifications.Any(curr => update.Guid == curr.Guid)).ToList();
                    addSpecification.ForEach(x => x.Guid = Guid.NewGuid());
                    var updateValue = from req in RequestSpecifications
                                      join curr in product.Specifications on req.Guid equals curr.Guid into joined
                                      from curr in joined.DefaultIfEmpty()
                                      where curr != null && !req.Equal(curr)
                                      select (curr, req);

                    foreach (var item in updateValue)
                    {
                        item.curr.IsEnable = item.req.IsEnable;
                        item.curr.Name = item.req.Name;
                        item.curr.Price = item.req.Price;
                        item.curr.DicountPrice = item.req.DicountPrice;
                        item.curr.Description = item.req.Description;
                        item.curr.UpdatedAt = DateTime.Now;
                        item.curr.UpdatedBy = "NA";
                    }
                    var UpdateSpecification = updateValue.Select(x => x.curr).ToList();
                    db.RemoveRange(removeSpecification);
                    db.UpdateRange(UpdateSpecification);
                    await db.AddRangeAsync(addSpecification);
                }


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

    [HttpPost(nameof(UploadImage))]
    public async Task<Result> UploadImage([FromQuery] ReqInfoImage reqInfo, [FromForm] IList<IFormFile> UploadFiles)
    {
        using (var db = _DbContext.Connection())
        {
            var product = await db.Set<EntityProduct>()
                .FirstOrDefaultAsync(x => x.Guid == reqInfo.Guid);
            if (product == null)
            {
                return "Không tìm thấy sản phẩm";
            }
            var images = product.Images.ToList() ?? new();
            foreach (var file in UploadFiles)
            {
                try
                {
                    using (var streamImage = file.OpenReadStream())
                    {
                        var image = await Image.LoadAsync(streamImage);
                        var guidFile = Guid.NewGuid();
                        var fouderRelLink = Path.Combine("Images", reqInfo.Guid.ToString());
                        var fouderLink = Path.Combine(WebRootPath, fouderRelLink);
                        var relLink = Path.Combine(fouderRelLink, guidFile.ToString() + $".jpgr");
                        if (!Directory.Exists(fouderLink))
                        {
                            Directory.CreateDirectory(fouderLink);
                        }
                        await image.SaveAsJpegAsync(Path.Combine(WebRootPath, relLink), new JpegEncoder()
                        {

                        });

                        images.Add(new()
                        {
                            FileName = file.FileName,
                            Guid = guidFile,
                            src = relLink
                        });
                    }
                }
                catch
                {
                }
            }
            product.Images = images;
            db.Update(product);
            await db.SaveChangesAsync();
            return Result.Ok();
        }
    }

    [HttpPost(nameof(UpdateImage))]
    public async Task<Result> UpdateImage([FromQuery] ReqInfoUpdateImage reqInfo, [FromForm] IFormFile UploadFile)
    {
        using (var db = _DbContext.Connection())
        {
            var product = await db.Set<EntityProduct>()
                .FirstOrDefaultAsync(x => x.Guid == reqInfo.Guid);
            if (product == null)
            {
                return "Không tìm thấy sản phẩm";
            }
            var imageUpdate = product.Images.FirstOrDefault(x => x.Guid == reqInfo.GuidImage);
            if (imageUpdate == null)
            {
                return "Không tìm thấy ảnh";
            }
            try
            {
                using (var streamImage = UploadFile.OpenReadStream())
                {
                    var image = await Image.LoadAsync(streamImage);
                    var fouderRelLink = Path.Combine("Images", reqInfo.Guid.ToString());
                    var fouderLink = Path.Combine(WebRootPath, fouderRelLink);
                    var relLink = Path.Combine(fouderRelLink, imageUpdate.Guid.ToString() + $".jpg");
                    if (!Directory.Exists(fouderLink))
                    {
                        Directory.CreateDirectory(fouderLink);
                    }
                    await image.SaveAsJpegAsync(Path.Combine(WebRootPath, imageUpdate.src), new JpegEncoder()
                    {

                    });
                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    [HttpPost(nameof(RemoveImage))]
    public async Task<Result> RemoveImage(ReqDeleteImages Request)
    {

        try
        {
            using (var db = _DbContext.Connection())
            {
                var proc = await db.Set<EntityProduct>()
                    .FirstOrDefaultAsync(x => x.Guid == Request.Guid);
                if (proc == null)
                {
                    return "Không tìm thấy sản phẩm";
                }
                var imagesToRemove = proc.Images.Where(x => Request.GuidImage.Contains(x.Guid)).ToList();
                foreach (var image in imagesToRemove)
                {
                    if (!System.IO.File.Exists(Path.Combine(WebRootPath, image.src)))
                    {
                        continue;
                    }
                    System.IO.File.Delete(Path.Combine(WebRootPath, image.src));
                }
                proc.Images = proc.Images.Where(x => !Request.GuidImage.Contains(x.Guid)).ToList();
                proc.CarouselImages = proc.CarouselImages?.Where(x => !Request.GuidImage.Contains(x.Guid)).ToList() ?? new();
                db.Update(proc);
                await db.SaveChangesAsync();
                return Result.Ok();
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
