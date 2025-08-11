using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract;

public interface ICurrentUser
{
    string UserId { get; }
    string UserName { get; }
    string Email { get; }
    string[] Roles { get; }
}
