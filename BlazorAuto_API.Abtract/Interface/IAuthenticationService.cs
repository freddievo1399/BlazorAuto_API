using System.Security.Claims;
using RestEase;

namespace BlazorAuto_API.Abstract
{
    [BasePath("/api/Auth/Authentication")]

    public interface IAuthentication
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

        [Post(nameof(LoginAsync))]
        Task<ResultOf<AuthResponse>> LoginAsync([Body]RequestLogin requestLogin);

        [Get(nameof(LoginAsync))]
        Task<ResultOf<AuthResponse>> UpdateAuthenticationAsync();

        [Get(nameof(LoginAsync))]
        Task<Result> Logout();

        [Get(nameof(GetInfo))]
        Task<ResultOf<ClaimsPrincipal>> GetInfo();

        [Get(nameof(CheckPermistion))]
        Task<Result> CheckPermistion();

        [Get(nameof(UpdateUser))]
        Task<Result> UpdateUser(ResultOf<AuthResponse> authResponse);
    }
}
