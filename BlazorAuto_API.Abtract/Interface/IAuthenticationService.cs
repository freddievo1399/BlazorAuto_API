using RestEase;

namespace BlazorAuto_API.Abstract
{
    [BasePath("/api/admin/Authentication")]

    public interface IAuthenticationService
    {
        [Post(nameof(LoginAsync))]
        Task<Result> LoginAsync([Body]RequestLogin requestLogin);
    }
}
