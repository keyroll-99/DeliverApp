using Microsoft.EntityFrameworkCore;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Request.Utils;
using Repository.Repository.Interface;
using Services.Interface.Utils;

namespace Services.Impl.Utils;

public class RoleUtils : IRoleUtils
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public RoleUtils(IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }

    public bool HasPermissionToAddUser(HasPermissionToAddUserRequest request)
    {
        if(request.LoggedUser is null)
        {
            throw new ArgumentException(ErrorMessage.LoggedUserIsNull);
        }

        var userRoles = request.LoggedUser.Roles;
        if (HasPermission(userRoles, SystemRoles.Admin))
        {
            return true;
        }

        return
            request.LoggedUserCompany is not null
            && request.TargetCompanyHash == request.LoggedUserCompany.Hash
            && (
               HasPermission(userRoles, SystemRoles.HR)
               || HasPermission(userRoles, SystemRoles.CompanyAdmin)
            );
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

    public bool HasPermissionToGetAllCompany(LoggedUser loggedUser)
    {
        return HasPermission(loggedUser.Roles, SystemRoles.Admin);
    }

    private static bool HasPermission(List<string> roles, string permission)
        => roles.Contains(permission);
}
