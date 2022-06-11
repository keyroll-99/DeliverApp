using Models.Request.User;
using Models.Response.User;

namespace Services.Interface;

public interface IUserService
{
    Task<UserResponse> CreateUser(CreateUserRequest createUserRequest);
    Task<List<UserResponse>> GetUserList();
    Task AddRoleToUser(Guid userHash, List<long> RolesId);
    Task AddUserToCompany(Guid userHash, Guid companyHash);
    Task UpdatePassword(ChangePasswordRequest updatePasswordRequest);
    Task<UserResponse> UpdateUser(UpdateUserRequest updateUserRequest);
    Task<UserResponse> GetUser(Guid userHash);
    Task FireUser(Guid userHash);
}
