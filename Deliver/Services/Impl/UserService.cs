using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request.User;
using Models.Response._Core;
using Models.Response.User;
using Repository.Repository.Interface;
using Services.Interface;

namespace Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;

        public UserService(IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        public async Task<User?> GetById(long id) =>
            await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(y => y.Role)
            .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<BaseResponse<AuthResponse>> Login(LoginRequest loginRequest, string ipAddress)
        {
            var user = await _userRepository
                .GetAll()
                .Include(x => x.UserRole)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == loginRequest.Username);

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BaseResponse<AuthResponse>.Fail("Invalid username or password");
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
                JwtToken = jwtTokne,
                Roles = user.UserRole.Select(x => x.Role.Name).ToList(),
            };
        }

        public async Task<BaseResponse<AuthResponse>> RefreshToken(string? token, string ipAddress)
        {
            var refreshToken = await _jwtUtils.GetRefreshTokenByToken(token);
            if (refreshToken is null)
            {
                return BaseResponse<AuthResponse>.Fail("Invalid token");
            }

            if (refreshToken.IsUsed)
            {
                await _jwtUtils.RevokeRefreshToken(refreshToken, ipAddress);
                return BaseResponse<AuthResponse>.Fail("Token already taken");
            }

            var user = await _userRepository
                .GetAll()
                .Include(x => x.UserRole)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);

            if (user is null)
            {
                return BaseResponse<AuthResponse>.Fail("Error with fetch user");
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
                JwtToken = jwtToken,
                Roles = user.UserRole.Select(x => x.Role.Name).ToList()
            };
        }
    }
}