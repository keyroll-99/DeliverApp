using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Exceptions;
using Models.Request.Authentication;
using Models.Response.Authentication;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;

namespace Services.Impl;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationUtils _authenticationUtils;
    private readonly IUserRepository _userRepository;
    private readonly LoggedUser _loggedUser;
    private readonly IRoleUtils _roleUtils;

    public AuthenticationService(
        IAuthenticationUtils authenticationUtils,
        IUserRepository userRepository,
        IOptions<LoggedUser> loggedUser,
        IRoleUtils roleUtils)
    {
        _authenticationUtils = authenticationUtils;
        _userRepository = userRepository;
        _loggedUser = loggedUser.Value;
        _roleUtils = roleUtils;
    }

    public Task<PermissionResponse> GetLoggedUserPermission()
        => _roleUtils.GetUserPermission(_loggedUser.Id);

    public async Task<AuthResponse> Login(LoginRequest loginRequest, string ipAddress)
    {

        var user = await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Username == loginRequest.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            throw new AppException(ErrorMessage.InvalidLoginOrPassword);
        }

        if (user.IsFired)
        {
            throw new AppException(ErrorMessage.UserIsBlocker);
        }

        var jwtToken = _authenticationUtils.GenerateJwtToken(user);
        var refreshToken = await _authenticationUtils.GenerateRefreshToken(user, ipAddress);

        return new AuthResponse
        {
            Hash = user.Hash,
            Name = user.Name,
            Surname = user.Surname,
            RefreshToken = refreshToken.Token,
            Username = user.Username,
            Jwt = jwtToken,
            CompanyHash = user.Company.Hash,
            ExpireDate = DateTime.Now.AddMinutes(15),
            Roles = user.UserRole.Select(x => x.Role.Name).ToList(),
        };
    }

    public async Task Logout(string ipAddress)
    {
        var user = await _userRepository.GetByIdAsync(_loggedUser.Id);
        await _authenticationUtils.RevokeAllRefreshTokenForUser(user, ipAddress);
    }

    public async Task<AuthResponse> RefreshToken(string? token, string ipAddress)
    {
        var refreshToken = await _authenticationUtils.GetRefreshTokenByToken(token);
        if (refreshToken is null)
        {
            throw new AppException(ErrorMessage.InvalidToken);
        }

        if (refreshToken.IsUsed)
        {
            await _authenticationUtils.RevokeRefreshToken(refreshToken, ipAddress);
            throw new AppException(ErrorMessage.TokenAlreadyTaken);
        }

        var user = await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);

        if (user is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        await _authenticationUtils.RevokeRefreshToken(refreshToken, ipAddress);
        refreshToken = await _authenticationUtils.GenerateRefreshToken(user, ipAddress);
        var jwtToken = _authenticationUtils.GenerateJwtToken(user);

        return new AuthResponse
        {
            Hash = user.Hash,
            Name = user.Name,
            RefreshToken = refreshToken.Token,
            Username = user.Username,
            Jwt = jwtToken,
            CompanyHash = user.Company.Hash,
            Surname = user.Surname,
            ExpireDate = DateTime.Now.AddMinutes(15),
            Roles = user.UserRole.Select(x => x.Role.Name).ToList()
        };
    }
}
