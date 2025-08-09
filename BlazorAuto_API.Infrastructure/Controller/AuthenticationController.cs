using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Infrastructure;
using BlazorAuto_API.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server;

[ApiController]
[Route("/api/admin/[controller]")]
[ApiExplorerSettings(GroupName = "Admin")]
public class AuthenticationController : ControllerBase, IAuthenticationService
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

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;
    public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<ApplicationRole> roleManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }
    [HttpPost(nameof(LoginAsync))]
    public async Task<Result> LoginAsync([FromBody] RequestLogin requestLogin)
    {
        var user = await _userManager.FindByNameAsync(requestLogin.UserName!);

        if (user == null)
        {
            return "K thấy user này";
        }


        var result = await _userManager.CheckPasswordAsync(user, requestLogin.Password!);

        if (!await _roleManager.RoleExistsAsync("hehe"))
        {

            var abc = await _roleManager.CreateAsync(new() { Name = "hehe" });
            await _userManager.AddToRoleAsync(user, "hehe");
        }
        if (user == null)
        {
            return "Khong thấy người dùng với tên đăng nhập này";
        }


        if (result)
        {

            var roleNames = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new()
                {
                    new Claim("sub", user.UserName!),
                    //new Claim("jti", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName!),
                };
            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                var role = await _roleManager.FindByNameAsync(roleName);
                var claim = await _roleManager.GetClaimsAsync(role);
                await _roleManager.FindByIdAsync(roleName);
                //ApplicationRole identityRole = new(); identityRole.Name
                claims.AddRange(claim);
            }




            var key = _config["Jwt:Key"];
            var issuer = _config["jwt:Issuer"];
            var audience = _config["jwt:Audience"];
            var expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("jwt:ExpireationInMinutes"));
            string nameAuthorizationCokie = _config["Jwt:AuthorizationName"]??"Token"; // đổi tên cookie cho đúng


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = issuer,
                Audience = audience
            };

            var handler = new JsonWebTokenHandler();
            var toke = handler.CreateToken(tokenDescriptor);

            Response.Cookies.Append(nameAuthorizationCokie, $"{toke}", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = expires
            });
            var context = _httpContextAccessor.HttpContext;
            return Result.Ok(toke);
        }

        return Result.Error("Mật khẩu sai");
    }
}

