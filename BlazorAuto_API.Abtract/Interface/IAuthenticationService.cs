using System.Security.Claims;
using RestEase;

namespace BlazorAuto_API.Abstract
{
    [BasePath("/api/Auth/Authentication")]

    public interface IAuthentication
    {
        [Post(nameof(LoginAsync))]
        Task<ResultOf<AuthResponse>> LoginAsync([Body] RequestLogin requestLogin);

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
