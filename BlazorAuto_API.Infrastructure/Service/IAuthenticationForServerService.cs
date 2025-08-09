using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Syncfusion.Blazor.Charts;

namespace BlazorAuto_API.Infrastructure
{
    public class IAuthenticationForServerService : IAuthenticationForServer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbContext _DbContext;

        public IAuthenticationForServerService(
            IDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _DbContext = db;
        }

        public async Task<ResultOf<AuthResponse>> LoginAsync(RequestLogin request)
        {
            var hasAnyUser = await _userManager.Users.AsNoTracking().AnyAsync();
            if (!hasAnyUser)
            {
                await CheckUserDefaul();
            }
            var user = await _userManager.FindByNameAsync(request.UserName!);
            if (user == null)
                return "User Name sai";
            if (!await _userManager.CheckPasswordAsync(user, request.Password!))
            {
                return "Sai password";
            }
            var roles = await _userManager.GetRolesAsync(user);

            var claims = await BuildClaimsAsync(user, roles);
            var (jwt, jwtExp) = GenerateJwt(claims);
            var refreshTokenValue = GenerateSecureToken();
            var userAgent = _httpContextAccessor.HttpContext!.Request.Headers["User-Agent"].ToString();
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            var expires = DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:ExpireationRefeshTokenInDays"));

            var refreshToken = new EntityRefreshTokenEntityBase
            {
                Token = refreshTokenValue,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ExpiryDate = expires,
                UserName = user.UserName!
            };

            using (var db = _DbContext.Connection())
            {
                db.Set<EntityRefreshTokenEntityBase>().Add(refreshToken);
                await db.SaveChangesAsync();
            }


            return new AuthResponse
            {
                AccessToken = jwt,
                AccessTokenExpiration = jwtExp,
                RefreshToken = refreshTokenValue,
                RefreshTokenExpiration = refreshToken.ExpiryDate
            };
        }

        public async Task<ResultOf<AuthResponse>> RefreshTokenAsync(string refreshTokenValue)
        {
            try
            {

                using (var db = _DbContext.Connection())
                {
                    var refreshToken = await db.Set<EntityRefreshTokenEntityBase>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Token == refreshTokenValue);

                    if (refreshToken == null)
                        return "Không tìm thấy token này";
                    if (refreshToken.ExpiryDate < DateTime.UtcNow)
                    {
                        db.Set<EntityRefreshTokenEntityBase>().Remove(refreshToken);
                        await db.SaveChangesAsync();
                        return "Token đã hết hạn, vui lòng đăng nhập lại để lấy token mới";
                    }
                    // Lấy user từ UserId
                    var user = await _userManager.FindByNameAsync(refreshToken.UserName);
                    if (user == null)
                        return "User này k tồn tại";

                    // Lấy role và claims
                    var roles = await _userManager.GetRolesAsync(user);
                    var claims = await BuildClaimsAsync(user, roles);

                    // Tạo lại JWT
                    var (jwt, jwtExp) = GenerateJwt(claims);

                    // (Tuỳ chọn) cập nhật thời gian mới hoặc tạo refresh token mới
                    // → ở đây: vẫn dùng lại refresh token cũ

                    return new AuthResponse
                    {
                        AccessToken = jwt,
                        AccessTokenExpiration = jwtExp,
                        RefreshToken = refreshToken.Token,
                        RefreshTokenExpiration = refreshToken.ExpiryDate
                    };
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        private async Task<List<Claim>> BuildClaimsAsync(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim("sub", user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

            foreach (var roleName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role!);

                foreach (var rc in roleClaims)
                {
                    var existing = claims.FirstOrDefault(x => x.Type == rc.Type);
                    if (existing != null)
                    {
                        var newValue = (int.Parse(existing.Value) | int.Parse(rc.Value)).ToString();
                        claims.Remove(existing);
                        claims.Add(new Claim(rc.Type, newValue));
                    }
                    else
                        claims.Add(rc);
                }
            }

            return claims;
        }

        private (string Token, DateTime Expires) GenerateJwt(List<Claim> claims)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var ExpireationInMinutes = _config.GetValue<int>("Jwt:ExpireationInMinutes");
            var expires = DateTime.UtcNow.AddMinutes(ExpireationInMinutes);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            claims.Add(new(ClaimTypes.Expired, expires.ToString()));
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, expires);
        }

        private string GenerateSecureToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
        public async Task CheckUserDefaul()
        {
            var UserName = "Admin";
            var Password = "@As123";
            var FullName = "Admin";
            var hasAnyUser = await _userManager.Users.AsNoTracking().AnyAsync(x => x.UserName == UserName);
            if (hasAnyUser)
                return;
            var user = new ApplicationUser { UserName = UserName, FullName = FullName };
            try
            {
                var resultCrateUser = await _userManager.CreateAsync(user, Password);
                if (!resultCrateUser.Succeeded)
                    throw new Exception("Lỗi tạo user mặc định: " + string.Join(", ", resultCrateUser.Errors.Select(e => e.Description)));
                var resultCrateRole = await _roleManager.CreateAsync(new ApplicationRole() { Name = "Admin" });
                if (!resultCrateRole.Succeeded)
                    throw new Exception("Lỗi tạo user mặc định: " + string.Join(", ", resultCrateRole.Errors.Select(e => e.Description)));
                var resultAddToRole = await _userManager.AddToRoleAsync(user, "Admin");
                if (!resultAddToRole.Succeeded)
                    throw new Exception("Lỗi thêm user vào role: " + string.Join(", ", resultAddToRole.Errors.Select(e => e.Description)));
                var claims = GlobalPermistion.GetClaims();
                var RoleAdmin = await _roleManager.FindByNameAsync("Admin");
                foreach (var claim in claims)
                {
                    var resultAddToClaim = await _roleManager.AddClaimAsync(RoleAdmin!, claim);
                    if (!resultAddToClaim.Succeeded)
                        throw new Exception("Lỗi thêm claim vào role: " + string.Join(", ", resultAddToClaim.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tạo user mặc định", ex);
            }
        }

        public async Task ClearRefeshToken()
        {
            using (var db = _DbContext.Connection())
            {
                var refreshToken = await db.Set<EntityRefreshTokenEntityBase>().Where(x => x.ExpiryDate < DateTime.UtcNow).ToListAsync();
                if (refreshToken.Any())
                {
                    db.Set<EntityRefreshTokenEntityBase>().RemoveRange(refreshToken);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}

