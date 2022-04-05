using Models.Db;
using Models.Request.utils;

namespace Services.Interface.Utils;

public interface IRoleUtils
{
    bool HasPermissionToAddUser(HasPermissionToAddUserRequest request);
    Task AddRolesToUser(User user , List<long> roleIds);
}
