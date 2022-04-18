using Models.Request.User;
using Models.Response.User;

namespace Services.Interface;

public interface IUserService
{
    Task<UserResponse> CreateUser(CreateUserRequest createUserRequest);
    Task AddRoleToUser(Guid userHash, List<long> RolesId);
    Task AddUserToCompany(Guid userHash, Guid companyHash);
}
