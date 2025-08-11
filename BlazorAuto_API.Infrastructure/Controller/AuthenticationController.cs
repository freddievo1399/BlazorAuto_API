using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server;

[ApiController]
[Route("/api/Auth/[controller]")]
[ApiExplorerSettings(GroupName = "Auth")]
[Authorize]
public class AuthenticationController : ControllerBase, IAuthentication
{
    private readonly IAuthenticationForServer _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;


    public AuthenticationController(IAuthenticationForServer authService, IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
        _config = config;
    }
    [AllowAnonymous]
    [HttpPost(nameof(LoginAsync))]
    public async Task<ResultOf<AuthResponse>> LoginAsync([FromBody] RequestLogin requestLogin)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return "Không tìm thấy HttpContext";
        }
        var result = await _authService.LoginAsync(requestLogin);
        if (result.Success)
        {
            return UpdateToken(result.Item, context);
        }
        return result.Message;
    }
    [AllowAnonymous]
    [HttpGet(nameof(UpdateAuthenticationAsync))]
    public async Task<ResultOf<AuthResponse>> UpdateAuthenticationAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return "Không tìm thấy HttpContext";
        }

        var hasRefeshToken = context.Request.Cookies.TryGetValue(_config["Jwt:RefeshTokenName"] ?? "TokenRefresh", out var refeshToken);
        if (!hasRefeshToken)
        {
            return "Không thấy TokenRefresh";
        }
        var result = await _authService.RefreshTokenAsync(refeshToken!);
        if (result.Success)
        {
            return UpdateToken(result.Item, context);
        }
        return result.Message;
    }
    private ResultOf<AuthResponse> UpdateToken(AuthResponse rps, HttpContext context)
    {
        try
        {
            string nameAuthorizationCokie = _config["Jwt:AuthorizationName"] ?? "Token";
            context.Response.Cookies.Append(nameAuthorizationCokie, $"{rps.AccessToken}", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = rps.AccessTokenExpiration,
            });
            string nameRefeshToken = _config["Jwt:RefeshTokenName"] ?? "TokenRefresh";

            context.Response.Cookies.Append(nameRefeshToken, $"{rps.RefreshToken}", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = rps.RefreshTokenExpiration,
            });
            return rps;
        }
        catch (Exception ex)
        {

            return ex.Message;
        }
    }
    [AllowAnonymous]
    [HttpGet(nameof(Logout))]
    public Task<Result> Logout()
    {
        try
        {
            var context = _httpContextAccessor.HttpContext;
            string nameAuthorizationCokie = _config["Jwt:AuthorizationName"] ?? "Token";
            var hasAuthorizationCokie = context.Request.Cookies.ContainsKey(nameAuthorizationCokie);
            if (hasAuthorizationCokie)
            {
                context.Response.Cookies.Delete(nameAuthorizationCokie);
            }

            string nameRefeshToken = _config["Jwt:RefeshTokenName"] ?? "TokenRefresh";
            var hasRefeshToken = context.Request.Cookies.ContainsKey(nameRefeshToken);
            if (hasAuthorizationCokie)
            {
                context.Response.Cookies.Delete(nameRefeshToken);
            }

            return Task.FromResult(Result.Ok());
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result.Error(ex.Message));
        }
    }
    [HttpGet(nameof(GetInfo))]
    public async Task<ResultOf<ClaimsPrincipal>> GetInfo()
    {
        return await Task.FromResult("Only user client side");
    }

    [HttpGet(nameof(CheckPermistion))]
    public async Task<Result> CheckPermistion()
    {
        return await Task.FromResult("Only user client side");
    }
    [HttpGet(nameof(UpdateUser))]

    public async Task<Result> UpdateUser(ResultOf<AuthResponse> authResponse)
    {
        return await Task.FromResult("Only user client side");
    }
}

