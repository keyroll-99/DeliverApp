using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request.Utils.Role;
using Repository.Repository.Interface;
using Services.Interface.Utils;

namespace Services.Impl.Utils;

public class RoleUtils : IRoleUtils
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;

    public RoleUtils(
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IRolePermissionRepository rolePermissionRepository)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    public async Task AddRolesToUser(User user, List<long> roleIds)
    {
        var roles = await _roleRepository.GetAll().Where(role => roleIds.Contains(role.Id)).ToListAsync();
        foreach (var role in roles)
        {
            var userRole = new UserRole
            {
                Role = role,
                User = user,
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userRoleRepository.AddAsync(userRole);
        }
    }

    public async Task<bool> HasPermission(HasPermissionRequest permissionRequest)
    {
        var rolesIds = await GetAllRolesIds(permissionRequest.Roles);

        var hasPermission =
            await (from permission in _rolePermissionRepository.GetAll()
                   where
                       rolesIds.Contains(permission.RoleId)
                       && permission.PermissionAction == permissionRequest.Action
                       && permission.PermissionTo == permissionRequest.PermissionTo
                   select permission).AnyAsync();

        return hasPermission;
    }

    private static bool HasPermission(List<string> roles, string permission)
        => roles.Contains(permission);

    private async Task<List<long>> GetAllRolesIds(List<string> roles)
        => await _roleRepository.GetAll().Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToListAsync();


}
