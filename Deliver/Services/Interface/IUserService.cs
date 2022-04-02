using Models.Db;
using Models.Request.User;
using Models.Response._Core;
using Models.Response.User;

namespace Services.Interface;

public interface IUserService
{
    Task<BaseResponse<AuthResponse>> Login(LoginRequest loginRequest, string ipAddress);
    Task<BaseResponse<AuthResponse>> RefreshToken(string? token, string ipAddress);
    Task<User?> GetById(long id);
}
