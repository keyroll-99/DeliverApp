using Models;
using Models.Db;
using Models.Request.Utils;
using Models.Request.Utils.Role;
using Models.Response.Authentication;

namespace Services.Interface.Utils;

public interface IRoleUtils
{
    Task<bool> HasPermission(HasPermissionRequest permissionRequest);
    Task AddRolesToUser(User user, List<long> roleIds);
    Task<PermissionResponse> GetUserPermission(long userId);
}
