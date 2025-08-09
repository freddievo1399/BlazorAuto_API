using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract.Model.Res;
using RestEase;

namespace BlazorAuto_API.Admin.Abstract
{
    [BasePath("/api/admin/ManagerUser")]

    public interface IManagerUser
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
        [Post(nameof(GetAllUsersAsync))]
        Task<PagedResultsOf<UserInfoModel>> GetAllUsersAsync([Body] DataRequestDto Request);

        [Post(nameof(CreateUser))]
        Task<Result> CreateUser([Body] UserCreateModel userCreateModel);

        [Get(nameof(GetUserByUserNameAsync))]
        Task<ResultOf<UserInfoModel>> GetUserByUserNameAsync(string UserName);
    }
}
