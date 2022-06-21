using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Db.ConstValues;
using Models.Request.Utils.Role;
using Models.Response.Authentication;
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

    public async Task<PermissionResponse> GetUserPermission(long userId)
    {
        var userRoles = await _roleRepository
            .GetAll()
            .Include(x => x.UserRoles)
            .Where(x => x.UserRoles.Any(y => y.UserId == userId))
            .ToListAsync();

        var result = new PermissionResponse
        {
            Company = new List<PermissionActionEnum>(),
            Deliver = new List<PermissionActionEnum>(),
            Location = new List<PermissionActionEnum>(),
            User = new List<PermissionActionEnum>(),
        };

        if (userRoles?.Any() == true)
        {
            foreach (var role in userRoles)
            {
                result.Company = (await GetAvailableAction(role.Id, PermissionToEnum.Company)).Concat(result.Company).Distinct().ToList();
                result.Location = (await GetAvailableAction(role.Id, PermissionToEnum.Location)).Concat(result.Location).Distinct().ToList();
                result.User = (await GetAvailableAction(role.Id, PermissionToEnum.User)).Concat(result.User).Distinct().ToList();
                result.Deliver = (await GetAvailableAction(role.Id, PermissionToEnum.Deliver)).Concat(result.Deliver).Distinct().ToList();
            }
        }

        return result;
    }

    private async Task<List<long>> GetAllRolesIds(List<string> roles)
        => await _roleRepository.GetAll().Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToListAsync();

    private async Task<List<PermissionActionEnum>> GetAvailableAction(long roleId, PermissionToEnum permissionTo)
    {
        return await _rolePermissionRepository
            .GetAll()
            .Where(x => x.PermissionTo == permissionTo && x.RoleId == roleId)
            .Select(x => x.PermissionAction)
            .ToListAsync();
    }
}
