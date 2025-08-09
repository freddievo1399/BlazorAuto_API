using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Admin.Abstract.Model;
using BlazorAuto_API.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server;
using static BlazorAuto_API.Admin.Abstract.IManagerRole;

[ApiController]
[Route("/api/admin/[controller]")]
[ApiExplorerSettings(GroupName = "Admin")]
public class ManagerRoleController : ControllerBase, IManagerRole
{


    private readonly IDbContext _DbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _config;
    public ManagerRoleController(IDbContext DbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration config)
    {
        _DbContext = DbContext;
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
    }
    [HttpPost(nameof(AddRole))]

    public async Task<Result> AddRole([FromBody] string roleName)
    {
        try
        {
            var RoleCurrent = await _roleManager.FindByNameAsync(roleName);
            if (RoleCurrent != null)
            {
                return Result.Error($"Role {roleName} đã tồn tại trong hệ thống. Vui lòng kiểm tra lại dữ liệu nhập vào.");
            }
            var role = new ApplicationRole()
            {
                Name = roleName,
            };
            var rls = await _roleManager.CreateAsync(role);
            if (rls.Succeeded)
            {
                return Result.Ok();
            }
            return string.Join(", ", rls.Errors.Select(e => e.Description));
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet(nameof(GetData))]
    public async Task<ResultsOf<Role>> GetData()
    {
        List<Role> rps = [];
        var Roles = await _roleManager.Roles.ToListAsync();
        foreach (var Role in Roles)
        {
            var role = new Role() { RoleName = Role.Name! };
            var clams = await _roleManager.GetClaimsAsync(Role);
            var users = await _userManager.GetUsersInRoleAsync(Role.Name!);

            role.UserNames = [.. users.Select(u => u.UserName!)];
            role.Permissions = [.. clams.Select(c => new Permission()
            {
                Name = c.Type,
                PermissionValue = Convert.ToInt32(c.Value)
            })];
            rps.Add(role);
        }
        return rps;
    }

    [HttpGet(nameof(GetFeature))]
    public async Task<ResultsOf<FeaturesModel>> GetFeature()
    {
        return await Task.FromResult(GlobalPermistion.FeaturesModels);
    }

    [HttpGet(nameof(GetUsers))]
    public async Task<ResultsOf<string>> GetUsers()
    {

        return await _userManager.Users.Select(u => u.UserName!).ToListAsync();
    }
    [HttpPost(nameof(RemoveRole))]

    public async Task<Result> RemoveRole([FromBody] string roleName)
    {
        try
        {
            var RoleCurrent = await _roleManager.FindByNameAsync(roleName);
            if (RoleCurrent == null)
            {
                return Result.Error($"Role {roleName} không tồn tại trong hệ thống. Vui lòng kiểm tra lại dữ liệu nhập vào.");
            }
            var rls = await _roleManager.DeleteAsync(RoleCurrent);
            if (rls.Succeeded)
            {
                return Result.Ok();
            }
            return string.Join(", ", rls.Errors.Select(e => e.Description));
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpPost(nameof(UpdateRole))]
    public async Task<Result> UpdateRole([FromBody] Role role)
    {
        var RoleCurrent = await _roleManager.FindByNameAsync(role.RoleName) ?? throw new Exception($"Role {role.RoleName} không tồn tại trong hệ thống. Vui lòng kiểm tra lại dữ liệu nhập vào.");

        //update user
        var userOld = await _userManager.GetUsersInRoleAsync(role.RoleName!);
        var userRemovesFromRole = userOld.Where(x => !role.UserNames.Contains(x.UserName!)).ToList();
        var userAddsToRole = role.UserNames.Where(x => !userOld.Any(u => u.UserName == x)).ToList();
        foreach (var user in userRemovesFromRole)
        {
            await _userManager.RemoveFromRoleAsync(user, role.RoleName!);
        }
        foreach (var user in userAddsToRole)
        {
            var userAdd = await _userManager.FindByNameAsync(user);
            if (userAdd != null)
            {
                await _userManager.AddToRoleAsync(userAdd, role.RoleName!);
            }
        }

        //update claims
        var claimCurrents = await _roleManager.GetClaimsAsync(RoleCurrent);
        var claimAdds = role.Permissions.Where(p => !claimCurrents.Any(c => c.Type == p.Name))
            .Select(permission => new System.Security.Claims.Claim(permission.Name, permission.PermissionValue.ToString())).ToList();
        var claimRemoves = claimCurrents.Where(c => !role.Permissions.Any(p => p.Name == c.Type)).ToList();
        var claimUpdates = from c in claimCurrents
                           join p in role.Permissions on c.Type equals p.Name
                           where c.Value != p.PermissionValue.ToString()
                           select (c, new System.Security.Claims.Claim(c.Type, p.PermissionValue.ToString()));
        foreach (var claim in claimRemoves)
        {
            await _roleManager.RemoveClaimAsync(RoleCurrent, claim);
        }
        foreach (var claim in claimAdds)
        {
            await _roleManager.AddClaimAsync(RoleCurrent, claim);
        }
        foreach (var (oldClaim, newClaim) in claimUpdates)
        {
            await _roleManager.RemoveClaimAsync(RoleCurrent, oldClaim);
            await _roleManager.RemoveClaimAsync(RoleCurrent, newClaim);
        }

        return Result.Ok("Cập nhật thành công các quyền hạn cho người dùng.");
    }
}
