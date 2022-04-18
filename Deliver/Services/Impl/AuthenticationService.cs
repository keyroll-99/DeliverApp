using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Request.Authentication;
using Models.Response.Authentication;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;

namespace Services.Impl;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtUtils _jwtUtils;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtUtils jwtUtils, IUserRepository userRepository)
    {
        _jwtUtils = jwtUtils;
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> Login(LoginRequest loginRequest, string ipAddress)
    {

        var user = await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == loginRequest.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            throw new AppException(ErrorMessage.InvalidLoginOrPassword);
        }

        var jwtTokne = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = await _jwtUtils.GenerateRefreshToken(user, ipAddress);

        return new AuthResponse
        {
            Hash = user.Hash,
            Name = user.Name,
            Surname = user.Surname,
            RefreshToken = refreshToken.Token,
            Username = user.Username,
            Jwt = jwtTokne,
            ExpireDate = DateTime.Now.AddMinutes(15),
            Roles = user.UserRole.Select(x => x.Role.Name).ToList(),
        };
    }

    public async Task<AuthResponse> RefreshToken(string? token, string ipAddress)
    {
        var refreshToken = await _jwtUtils.GetRefreshTokenByToken(token);
        if (refreshToken is null)
        {
            throw new AppException(ErrorMessage.InvalidToken);
        }

        if (refreshToken.IsUsed)
        {
            await _jwtUtils.RevokeRefreshToken(refreshToken, ipAddress);
            throw new AppException(ErrorMessage.TokenAlreadyTaken);
        }

        var user = await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);

        if (user is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        await _jwtUtils.RevokeRefreshToken(refreshToken, ipAddress);
        refreshToken = await _jwtUtils.GenerateRefreshToken(user, ipAddress);
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthResponse
        {
            Hash = user.Hash,
            Name = user.Name,
            RefreshToken = refreshToken.Token,
            Username = user.Username,
            Jwt = jwtToken,
            ExpireDate = DateTime.Now.AddMinutes(15),
            Roles = user.UserRole.Select(x => x.Role.Name).ToList()
        };
    }
}
