using Integrations.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Exceptions;
using Models.Integration;
using Models.Request.User;
using Models.Request.utils;
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

        public async Task<UserReponse> CreateUser(CreateUserRequest createUserRequest)
        {
            if (createUserRequest is null || !createUserRequest.IsValid)
            {
                throw new AppException(ErrorMessage.InvalidData);
            }

            if(!(await VerifyPerrmisionToAddUser(createUserRequest.CompanyHash)))
            {
                throw new AppException(ErrorMessage.InvalidRole);
            }

            var company = await _companyUtils.GetCompanyByHash(createUserRequest.CompanyHash);
            if (company is null)
            {
                throw new AppException(ErrorMessage.CompanyDoesntExists);
            }

            var newUser = await AddUserAndSendMail(createUserRequest.AsUser(), company);

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

        private async Task<bool> VerifyPerrmisionToAddUser(Guid hash)
        {
            var userCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);
            return _roleUtils.HasPermissionToAddUser(new HasPermissionToAddUserRequest
            {
                LoggedUser = _loggedUser,
                LoggedUserCompany = userCompany,
                TargetCompanyHash = hash
            });
        }

        private async Task<User> AddUserAndSendMail(User user, Company company)
        {
            var password = Convert.ToBase64String(RandomNumberGenerator.GetBytes(5)).Replace("=", "");
            user.Hash = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.CompanyId = company.Id;

            await _userRepository.AddAsync(user);
            await SendWelcomeMail(user, password);
            return user;
        }

        private async Task SendWelcomeMail(User user, string password)
        {
            await _mailService.SendWelcomeMessage(new WelcomeMessageModel
            {
                Email = user.Email,
                Name = user.Name,
                Password = password,
                Surname = user.Surname,
                Username = user.Username
            });
        }

        public async Task AddRoleToUser(Guid userHash, List<long> RolesId)
        {
            var user = await _userRepository.GetByHashAsync(userHash);
            if (user is null)
            {
                throw new AppException(ErrorMessage.UserDosentExists);
            }
            await _roleUtils.AddRolesToUser(user, RolesId);
        }

        public async Task AddUserToCompany(Guid userHash, Guid companyHash)
        {
            var company = await _companyUtils.GetCompanyByHash(companyHash);
            var user = await _userRepository.GetByHashAsync(userHash);

            if (company is null)
            {
                throw new AppException(ErrorMessage.CompanyDoesntExists);
            }
            if (user is null)
            {
                throw new AppException(ErrorMessage.UserDosentExists);
            }

            user.Company = company;
            user.CompanyId = company.Id;

            await _userRepository.UpdateAsync(user);
        }
    }
}