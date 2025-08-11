using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.AspNetCore.Components.Forms;
using RestEase;
using Syncfusion.Blazor;

namespace BlazorAuto_API.Admin.Abstract
{
    [BasePath("/api/admin/productdetail")]
    public interface IProductDetailService
    {
        [Feature("Danh mục", "Cấu hình sản phẩm")]
        public enum PERMISSION
        {
            [Permistion("Xem danh sách")]
            ALLOW_VIEW,

            [Permistion("Thêm")]
            ALLOW_ADD,

            [Permistion("Sửa")]
            ALLOW_UPDATE,

            [Permistion("Xóa")]
            ALLOW_DELETE,
        }
        [Get(nameof(FindByGuid))]
        Task<ResultOf<ProductDetailModel>> FindByGuid(Guid Guid);

        [Get(nameof(GetListCategory))]
        Task<ResultsOf<CategoriesInfoModel>> GetListCategory();

        [Post(nameof(Update))]
        Task<Result> Update([Body] ProductUpdateModel Request);

        [Post(nameof(RemoveImage))]
        Task<Result> RemoveImage([Body] ReqDeleteImages Request);
    }
}
