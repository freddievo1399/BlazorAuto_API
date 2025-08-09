using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorAuto_API.Abstract;
using BlazorAuto_API.Admin.Abstract;
using BlazorAuto_API.Infrastructure;
using BlazorAuto_API.Infrastructure.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Admin.Server;

[ApiController]
[Route("/api/admin/[controller]")]
[ApiExplorerSettings(GroupName = "Admin")]
public class ManagerRoleController : ControllerBase, IManagerRole
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
        }
        return rps;
    }

    [HttpGet(nameof(GetUsers))]
    public async Task<ResultsOf<string>> GetUsers()
    {

        return await _userManager.Users.Select(u => u.UserName!).ToListAsync();
    }

    [HttpPost(nameof(UpdateRole))]
    public async Task<Result> UpdateRole([FromBody] List<Role> roles)
    {
        var roleCurrents = (await _roleManager.Roles.ToListAsync()) ?? [];

        roleCurrents = null;
        //Xóa role
        var roleRemoves = roleCurrents?.Where(r => !roles.Any(x => x.RoleName == r.Name)).ToList() ?? [];
        foreach (var roleRemove in roleRemoves)
        {
            await _roleManager.DeleteAsync(roleRemove);
            roleCurrents!.Remove(roleRemove);
        }
        roleRemoves = null;

        //thêm mới role
        var roleAdds = roles.Where(r => !roleCurrents!.Any(x => x.Name == r.RoleName)).ToList();
        foreach (var role in roleAdds)
        {
            var newRole = new ApplicationRole { Name = role.RoleName };
            await _roleManager.CreateAsync(newRole);
            var users = await _userManager.Users.Where(x => role.UserNames.Contains(x.UserName!)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.AddToRoleAsync(user, role.RoleName);
            }
            foreach (var permission in role.Permissions)
            {
                await _roleManager.AddClaimAsync(newRole, new System.Security.Claims.Claim(permission.Name, permission.PermissionValue.ToString()));
            }
        }
        roleAdds = null;


        //thêm mới Cập nhật
        var rolUpdates = roles.Where(r => roleCurrents!.Any(x => x.Name == r.RoleName)).ToList();
        foreach (var role in rolUpdates)
        {
            var RoleCurrent = await _roleManager.FindByNameAsync(role.RoleName) ?? throw new Exception($"Role {role.RoleName} không tồn tại trong hệ thống. Vui lòng kiểm tra lại dữ liệu nhập vào.");
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

        }
        return Result.Ok("Cập nhật thành công các quyền hạn cho người dùng.");
    }
}
