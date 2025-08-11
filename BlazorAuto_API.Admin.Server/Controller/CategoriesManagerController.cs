using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Diagram;
using static BlazorAuto_API.Admin.Abstract.ICategoriesManagerService;
namespace BlazorAuto_API.Admin.Server
{
    [ApiController]
    [Route("/api/admin/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [Authorize]
    public class CategoriesManagerController : ControllerBase, ICategoriesManagerService
    {
        
        IDbContext _DbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal UserInfo => _httpContextAccessor?.HttpContext?.User ?? new();
        public CategoriesManagerController(IDbContext DbContext, IHttpContextAccessor httpContextAccessor)
        {
            _DbContext = DbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost(nameof(Add))]
        public async Task<Result> Add([FromBody] CategoriesCreateModel Request)
        {
            if (!UserInfo.HasPermission(PERMISSION.ALLOW_VIEW, PERMISSION.ALLOW_ADD))
            {
                return "Bạn không có quyền truy cập dữ liệu này.";
            }
            try
            {
                using (var db = _DbContext.Connection())
                {
                    var abc = new EntityCategory()
                    {
                        Name = Request.Name,
                        Description = Request.Description,
                        CreatedBy = "test"
                    };
                    await db.Set<EntityCategory>().AddAsync(abc);
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
        public async Task<ResultOf<CategoriesInfoModel>> FindByGuid(Guid Guid)
        {
            if (!UserInfo.HasPermission(PERMISSION.ALLOW_VIEW))
            {
                return "Bạn không có quyền truy cập dữ liệu này.";
            }
            try
            {
                using (var db = _DbContext.Connection())
                {
                    var res = await db.Set<EntityCategory>().Include(x => x.ProductCategories).ThenInclude(x => x.Product).Select(x => new CategoriesInfoModel()
                    {
                        Guid = Guid,
                        Name = x.Name,
                        Description = x.Description,
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
        public async Task<PagedResultsOf<CategoriesInfoModel>> GetData([FromBody] DataRequestDto Request)
        {
            if (!UserInfo.HasPermission(PERMISSION.ALLOW_VIEW))
            {
                return "Bạn không có quyền truy cập dữ liệu này.";
            }
            try
            {
                var rstTemp = await _DbContext.GetData<EntityCategory>(Request, x => x.Where(x => !x.DeletedAt.HasValue).Include(x => x.ProductCategories).ThenInclude(x => x.Product));
                var rst = PagedResultsOf<CategoriesInfoModel>.Ok(
                    rstTemp.Items.Select(x => new CategoriesInfoModel
                    {
                        Description = x.Description,
                        Guid = x.Guid,
                        Name = x.Name,
                    }), rstTemp.TotalCount);
                return rst;
            }
            catch (Exception)
            {
                return PagedResultsOf<CategoriesInfoModel>.Error("An error occurred while fetching data.");
            }



        }
        [HttpPost(nameof(Remove))]
        public async Task<Result> Remove(Guid Guid)
        {
            try
            {
                if (!UserInfo.HasPermission(PERMISSION.ALLOW_VIEW,PERMISSION.ALLOW_DELETE))
                {
                    return "Bạn không có quyền truy cập dữ liệu này.";
                }
                using (var db = _DbContext.Connection())
                {
                    var model = await db.Set<EntityCategory>().FirstOrDefaultAsync(x => x.Guid == Guid);
                    if (model == null)
                    {
                        return "Không tìm thấy";
                    }
                    model.DeletedBy = "test";
                    model.DeletedAt = DateTime.Now;
                    db.Set<EntityCategory>().Update(model);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
