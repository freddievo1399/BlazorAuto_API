using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RestEase;
using WebApp.Client;

namespace WebApp
{
    public class AuthenticationForServerService(IJSRuntime jS, IHttpContextAccessor httpContextAccessor, IConfiguration config) : AuthenticationForClientService(jS), IAuthentication
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IConfiguration config = config;

        public async new Task<Result> CheckPermistion()
        {
            var user = await GetInfo();

            var expiredClaim = user.Item.FindFirst(ClaimTypes.Expired)?.Value;

            if (DateTime.TryParse(expiredClaim, out var expires))
            {
                if (expires < DateTime.UtcNow)
                {
                    var result = await UpdateAuthenticationAsync();
                    if (!result.Success)
                    {
                        return Result.Error(result.Message);
                    }
                }
                return Result.Ok();
            }
            return Result.Error("");//Error jwt error not has expired
        }

        public new async Task<ResultOf<ClaimsPrincipal>> GetInfo()
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false
                ? ResultOf<ClaimsPrincipal>.Ok(httpContextAccessor.HttpContext.User)
                : await base.GetInfo();
        }

        public new async Task<ResultOf<AuthResponse>> LoginAsync(RequestLogin requestLogin)
        {
            var result = await base.LoginAsync(requestLogin);
            if (result.Success)
            {
                await UpdateUser(result);
                return result;
            }
            return result;
        }

        public new async Task<Result> Logout()
        {
            httpContextAccessor.HttpContext!.User = new ClaimsPrincipal(new ClaimsIdentity());
            await httpContextAccessor.HttpContext!.SignOutAsync();
            return await base.Logout();
        }

        public new async Task<ResultOf<AuthResponse>> UpdateAuthenticationAsync()
        {
            var result = await base.UpdateAuthenticationAsync();
            if (result.Success)
            {
                await UpdateUser(result);
                return result;
            }
            return result;
        }
        public new async Task<Result> UpdateUser(ResultOf<AuthResponse> authResponse)
        {
            var result = await base.UpdateUser(authResponse);
            var key = config["Jwt:Key"];
            httpContextAccessor.HttpContext!.User = JwtHelper.CreatePrincipalFromJsonWithKey(authResponse.Item!.AccessToken, key!);
            //await httpContextAccessor.HttpContext!.SignInAsync(httpContextAccessor.HttpContext!.User);
            return result;
        }
    }
}
