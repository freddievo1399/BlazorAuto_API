using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;

namespace BlazorAuto_API.Infrastructure
{
    public interface IAuthenticationForServer
    {
        Task<ResultOf<AuthResponse>> LoginAsync(RequestLogin request);
        Task<ResultOf<AuthResponse>> RefreshTokenAsync(string refreshToken);
        Task CheckUserDefaul();
        Task ClearRefeshToken();
    }
}
