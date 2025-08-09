using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RestEase;

namespace WebApp.Client
{
    public class AuthenticationForClientService(IJSRuntime jS) : IAuthentication
    {
        private IJSRuntime JS = jS;

        public async Task<Result> CheckPermistion()
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
            return Result.Ok();
        }

        public async Task<ResultOf<ClaimsPrincipal>> GetInfo()
        {
            var json = await JS.InvokeAsync<string>("localStorage.getItem", "InfoUser");
            return JwtHelper.CreatePrincipalFromJson(json);
        }

        public async Task<ResultOf<AuthResponse>> LoginAsync(RequestLogin requestLogin)
        {
            var jsonElement = await JS.InvokeAsync<System.Text.Json.JsonElement>("callApi",
            $"/api/Auth/Authentication/{nameof(LoginAsync)}",
            "POST",
            requestLogin
        );
            var resultString = jsonElement.GetRawText();
            var result = JsonConvert.DeserializeObject<ResultOf<AuthResponse>>(resultString) ?? "Lỗi ép kiểu";
            await UpdateUser(result);
            return result;
        }

        public async Task<Result> Logout()
        {
            await JS.InvokeAsync<Result>("localStorage.removeItem", "InfoUser");
            return Result.Ok();
        }

        public async Task<ResultOf<AuthResponse>> UpdateAuthenticationAsync()
        {
            var jsonElement = await JS.InvokeAsync<System.Text.Json.JsonElement>("callApi",
            $"/api/Auth/Authentication/{nameof(UpdateAuthenticationAsync)}",
            "GET"
            );
            var resultString = jsonElement.GetRawText();
            var result = JsonConvert.DeserializeObject<ResultOf<AuthResponse>>(resultString) ?? "Lỗi ép kiểu";
            await UpdateUser(result);
            return result;
        }


        public async Task<Result> UpdateUser(ResultOf<AuthResponse> authResponse)
        {
            try
            {

                if (authResponse.Success)
                {
                    var InfoUser = JwtHelper.GetJsonPrincipal(authResponse.Item.AccessToken);
                    await JS.InvokeVoidAsync("localStorage.setItem", "InfoUser", InfoUser);
                }
                else
                {
                    await JS.InvokeVoidAsync("localStorage.removeItem", "InfoUser");
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
