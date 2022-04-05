using Models.Request.User;
using Models.Response._Core;
using Models.Response.User;

namespace Services.Interface;

public interface IUserService
{
    Task<BaseRespons<AuthResponse>> Login(LoginRequest loginRequest, string ipAddress);
    Task<BaseRespons<AuthResponse>> RefreshToken(string? token, string ipAddress);
    Task<BaseRespons<UserReponse>> CreateUser(CreateUserRequest createUserRequest);
    Task AddRoleToUser(Guid userHash, List<long> RolesId);
    Task AddUserToCompany(Guid userHash, Guid companyHash);
}
