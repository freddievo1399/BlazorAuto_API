using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server;
using static BlazorAuto_API.Admin.Abstract.IManagerUser;

[ApiController]
[Route("/api/admin/[controller]")]
[ApiExplorerSettings(GroupName = "Admin")]
public class ManagerUserController : ControllerBase, IManagerUser
{
    private readonly IDbContext _DbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _config;
    public ManagerUserController(IDbContext DbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration config)
    {
        _DbContext = DbContext;
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
    }

    [HttpPost(nameof(GetAllUsersAsync))]
    public async Task<PagedResultsOf<UserInfoModel>> GetAllUsersAsync(DataRequestDto Request)
    {
        try
        {
            var TotalCount = await _userManager.Users.CountAsync();
            var query = _userManager.Users;
            if (Request.Take>0)
            {
                query = query.Take(Request.Take ?? 0);
            }
            if (Request.Skip > 0)
            {
                query = query.Skip(Request.Skip ?? 0);
            }
            var rstTemp = query.ToList();

            var rst = PagedResultsOf<UserInfoModel>.Ok(
                rstTemp.Select(x => new UserInfoModel()
                {
                    UserName = x.UserName!,
                    FullName = x.FullName!,
                }), TotalCount);
            return rst;
        }
        catch (Exception ex)
        {

            return ex.Message;
        }

    }
    [HttpGet(nameof(GetUserByUserNameAsync))]

    public Task<ResultOf<UserInfoModel>> GetUserByUserNameAsync(string UserName)
    {
        throw new NotImplementedException();
    }
    [HttpPost(nameof(CreateUser))]

    public async Task<Result> CreateUser(UserCreateModel userCreateModel)
    {
        if (userCreateModel.Password != userCreateModel.PasswordVerify)
        {
            return "Pass không trùng";
        }

        var user = new ApplicationUser { UserName = userCreateModel.UserName, FullName = userCreateModel.FullName };
        try
        {
            var userDuplidate = await _userManager.FindByNameAsync(userCreateModel.UserName!);
            if (userDuplidate != null)
            {
                return "Tồn tại userName này rồi";
            }
            var result = await _userManager.CreateAsync(user, userCreateModel.Password!);
            if (result.Succeeded)
                return Result.Ok();
            return Result.Error(JsonConvert.SerializeObject(result.Errors));

        }
        catch (Exception ex)
        {
            return Result.Error($"Đăng ký thất bại: {ex.Message}");
        }
    }
}
