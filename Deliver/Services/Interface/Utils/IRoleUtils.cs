using Models;
using Models.Db;
using Models.Request.Utils;

namespace Services.Interface.Utils;

public interface IRoleUtils
{
    bool HasPermissionToGetAllCompany(LoggedUser loggedUser);
    bool HasPermissionToUserAction(HasPermissionToActionOnUserRequest request);
    Task AddRolesToUser(User user, List<long> roleIds);
}
