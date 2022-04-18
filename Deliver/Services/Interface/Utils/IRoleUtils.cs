using Models;
using Models.Db;
using Models.Request.Utils;

namespace Services.Interface.Utils;

public interface IRoleUtils
{
    bool HasPermissionToAddUser(HasPermissionToAddUserRequest request);
    bool HasPermissionToGetAllCompany(LoggedUser loggedUser);
    Task AddRolesToUser(User user , List<long> roleIds);
}
