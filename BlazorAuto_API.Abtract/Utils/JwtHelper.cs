using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    public static string CreateUnsignedJwt(List<Claim> claims)
    {
        // Header (alg: none)
        var header = new { alg = "none", typ = "JWT" };
        string headerJson = JsonConvert.SerializeObject(header);
        string headerBase64 = Base64UrlEncode(headerJson);

        // Payload (from claims)
        var payloadDict = new Dictionary<string, string>();
        foreach (var claim in claims)
        {
            payloadDict[claim.Type] = claim.Value;
        }
        string payloadJson = JsonConvert.SerializeObject(payloadDict);
        string payloadBase64 = Base64UrlEncode(payloadJson);

        // Combine (no signature)
        return $"{headerBase64}.{payloadBase64}.";
    }

    private static string Base64UrlEncode(string input)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(input))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
