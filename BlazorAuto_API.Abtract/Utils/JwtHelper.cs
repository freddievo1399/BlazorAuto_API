using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BlazorAuto_API.Abstract;

public static class JwtHelper
{
    public static ClaimsPrincipal CreatePrincipalFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        var principal = new ClaimsPrincipal(identity);
        return principal;
    }
    public static string GetJsonPrincipal(string token)
    {
        var user = CreatePrincipalFromToken(token);
        var identity = (ClaimsIdentity)user.Identity!;

        var claimsList = identity.Claims
            .Select(c => new Dictionary<string, string>
            {
                ["Type"] = c.Type,
                ["Value"] = c.Value
            }).ToList();

        return JsonConvert.SerializeObject(claimsList);
    }
    public static ClaimsPrincipal CreatePrincipalFromJson(string jsonData)
    {
        if (jsonData == null)
        {
            return new();
        }
        var claimDictList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonData);

        if (claimDictList == null) return new ClaimsPrincipal();

        var claims = claimDictList.Select(d => new Claim(d["Type"], d["Value"]));
        var identity = new ClaimsIdentity(claims);
        return new ClaimsPrincipal(identity);
    }
    public static ClaimsPrincipal CreatePrincipalFromJsonWithKey(string Token, string secretKey)
    {
        if (Token == null)
        {
            return new();
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // bỏ nếu chỉ muốn đọc
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero // bỏ trễ
        };

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(Token, tokenValidationParameters, out var validatedToken);

            // Kiểm tra có đúng JWT không
            if (validatedToken is JwtSecurityToken jwt &&
                jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return principal;
            }
        }
        catch (Exception)
        {
            return new();
        }

        return new();
    }
}
