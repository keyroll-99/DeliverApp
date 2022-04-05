using Integrations.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Exceptions;
using Models.Integration;
using Models.Request.User;
using Models.Request.utils;
using Models.Response._Core;
using Models.Response.User;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;
using System.Security.Cryptography;

namespace Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly ICompanyUtils _companyUtils;
        private readonly LoggedUser _loggedUser;
        private readonly IRoleUtils _roleUtils;
        private readonly IMailService _mailService;

        public UserService(
            IUserRepository userRepository,
            IJwtUtils jwtUtils,
            ICompanyUtils companyUtils,
            IOptions<LoggedUser> loggedUser,
            IRoleUtils roleUtils,
            IMailService mailService)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
            _companyUtils = companyUtils;
            _loggedUser = loggedUser.Value;
            _roleUtils = roleUtils;
            _mailService = mailService;
        }

        public async Task<BaseRespons<AuthResponse>> Login(LoginRequest loginRequest, string ipAddress)
        {

            var user = await _userRepository
                .GetAll()
                .Include(x => x.UserRole)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == loginRequest.Username);

            if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return BaseRespons<AuthResponse>.Fail("Invalid username or password");
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

        public async Task<BaseRespons<AuthResponse>> RefreshToken(string? token, string ipAddress)
        {
            var refreshToken = await _jwtUtils.GetRefreshTokenByToken(token);
            if (refreshToken is null)
            {
                return BaseRespons<AuthResponse>.Fail("Invalid token");
            }

            if (refreshToken.IsUsed)
            {
                await _jwtUtils.RevokeRefreshToken(refreshToken, ipAddress);
                return BaseRespons<AuthResponse>.Fail("Token already taken");
            }

            var user = await _userRepository
                .GetAll()
                .Include(x => x.UserRole)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);

            if (user is null)
            {
                return BaseRespons<AuthResponse>.Fail("Error with fetch user");
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

        public async Task<BaseRespons<UserReponse>> CreateUser(CreateUserRequest createUserRequest)
        {
            if (createUserRequest is null || !createUserRequest.IsValid)
            {
                return BaseRespons<UserReponse>.Fail("Invalid Data");
            }

            var userCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);
            var hasPerrmisionToAdd = _roleUtils.HasPermissionToAddUser(new HasPermissionToAddUserRequest
                {
                    LoggedUser = _loggedUser,
                    LoggedUserCompany = userCompany,
                    TargetCompanyHash = createUserRequest.CompanyHash
                });

            if (!hasPerrmisionToAdd)
            {
                return BaseRespons<UserReponse>.Fail("Invalid roles");
            }

            var company = await _companyUtils.GetCompanyByHash(createUserRequest.CompanyHash);
            if (company is null)
            {
                return BaseRespons<UserReponse>.Fail("Invalid company");
            }
            
            var password = Convert.ToBase64String(RandomNumberGenerator.GetBytes(5)).Replace("=", "");
            var newUser = new User
                {
                    Hash = Guid.NewGuid(),
                    Name = createUserRequest.Name,
                    Surname = createUserRequest.Surname,
                    CompanyId = company.Id,
                    Email = createUserRequest.Email,
                    PhoneNumber = createUserRequest.PhoneNumber,
                    Username = createUserRequest.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                };

            await _userRepository.AddAsync(newUser);
            
            await _mailService.SendWelcomeMessage(new WelcomeMessageModel
                {
                    Email = createUserRequest.Email,
                    Name = createUserRequest.Name,
                    Password = password,
                    Surname = createUserRequest.Surname,
                    Username = createUserRequest.Username
                });

            return new UserReponse
            {
                CompanyHash = company.Hash,
                CompanyName = company.Name,
                Hash = newUser.Hash,
                Email = newUser.Email,
                Name = newUser.Name,
                Surname = newUser.Surname,
                Username = newUser.Username,
            };
        }

        public async Task AddRoleToUser(Guid userHash, List<long> RolesId)
        {
            var user = await _userRepository.GetByHashAsync(userHash);
            if(user is null)
            {
                throw new AppException(ErrorMessage.UserDosentExists);
            }
            await _roleUtils.AddRolesToUser(user, RolesId);
        }

        public async Task AddUserToCompany(Guid userHash, Guid companyHash)
        {
            var company = await _companyUtils.GetCompanyByHash(companyHash);
            var user = await _userRepository.GetByHashAsync(userHash);
            
            if(company is null)
            {
                throw new AppException(ErrorMessage.CompanyDoesntExists);
            }
            if(user is null)
            {
                throw new AppException(ErrorMessage.UserDosentExists);
            }

            user.Company = company;
            user.CompanyId = company.Id;
            
            await _userRepository.UpdateAsync(user);
        }
    }
}