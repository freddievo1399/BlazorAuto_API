using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract.Model;
using RestEase;

namespace BlazorAuto_API.Admin.Abstract
{
    [BasePath("/api/admin/ManagerRole")]

    public interface IManagerRole
    {
        [Feature("Danh mục", "Quản lý danh mục")]
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
        [Get(nameof(GetData))]
        Task<ResultsOf<Role>> GetData();

        [Get(nameof(GetUsers))]
        Task<ResultsOf<string>> GetUsers();

        [Get(nameof(GetFeature))]
        Task<ResultsOf<FeaturesModel>> GetFeature();
        

        [Post(nameof(UpdateRole))]
        Task<Result> UpdateRole([Body] Role role);

        [Post(nameof(AddRole))]
        Task<Result> AddRole([Body] string roleName);

        [Post(nameof(RemoveRole))]
        Task<Result> RemoveRole([Body] string roleName);
    }
}
