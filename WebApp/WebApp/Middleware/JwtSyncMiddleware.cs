using Microsoft.CodeAnalysis.Differencing;

namespace WebApp.Middleware
{
    public class JwtSyncMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtSyncMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var nameAuthorizationCokie = _config["Jwt:AuthorizationName"] ; // đổi tên cookie cho đúng
            var expires = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("jwt:ExpireationInMinutes"));
            var hasAuthHeader = context.Request.Headers.TryGetValue("Authorization", out var authHeaderToken);
            var hasAuthCookies = context.Request.Cookies.TryGetValue(nameAuthorizationCokie, out var authCookieToken);
            var authToken = hasAuthHeader ? authHeaderToken.ToString().Replace("Bearer ", "") : null;

            if (hasAuthCookies)
            {
                if (authHeaderToken== authCookieToken)
                {

                }
                else
                {
                    context.Request.Headers["Authorization"] = $"Bearer {authCookieToken}";
                }
            }
            else if(hasAuthHeader && !string.IsNullOrEmpty(authToken))
            {
                context.Response.Cookies.Append(nameAuthorizationCokie, authToken.Replace("Bearer ",""), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = expires
                });
            }

            await _next(context);
        }
    }

}
