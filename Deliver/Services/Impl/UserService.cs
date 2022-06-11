using Integrations.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Integration;
using Models.Mapper;
using Models.Request.User;
using Models.Request.Utils.Role;
using Models.Response.User;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;
using System.Security.Cryptography;

namespace Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICompanyUtils _companyUtils;
    private readonly LoggedUser _loggedUser;
    private readonly IRoleUtils _roleUtils;
    private readonly IMailService _mailService;

    public UserService(
        IUserRepository userRepository,
        ICompanyUtils companyUtils,
        IOptions<LoggedUser> loggedUser,
        IRoleUtils roleUtils,
        IMailService mailService)
    {
        _userRepository = userRepository;
        _companyUtils = companyUtils;
        _loggedUser = loggedUser.Value;
        _roleUtils = roleUtils;
        _mailService = mailService;
    }

    public async Task<UserResponse> CreateUser(CreateUserRequest createUserRequest)
    {
        if (createUserRequest is null || !createUserRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if ((await _userRepository.GetAll().AnyAsync(x => x.Username == createUserRequest.Username)))
        {
            throw new AppException(ErrorMessage.UserExists);
        }

        if (!(await VerifyPerrmisonToActionOnUser(createUserRequest.CompanyHash, PermissionActionEnum.Create)))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        var company = await _companyUtils.GetCompanyByHash(createUserRequest.CompanyHash);
        if (company is null)
        {
            throw new AppException(ErrorMessage.CompanyDoesntExists);
        }

        var user = createUserRequest.AsUser();

        var password = GeneratePasssword();

        var addUserTask = AddUser(user, company, password);
        var sendEmailTask = SendWelcomeMail(user, password);

        Task.WaitAll(addUserTask, sendEmailTask);

        var newUser = addUserTask.Result;

        return new UserResponse
        {
            CompanyHash = company.Hash,
            CompanyName = company.Name,
            Hash = newUser.Hash,
            Email = newUser.Email,
            Name = newUser.Name,
            Surname = newUser.Surname,
            PhoneNumber = newUser.PhoneNumber,
            Username = newUser.Username,
        };
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

    public async Task UpdatePassword(ChangePasswordRequest updatePasswordRequest)
    {
        if (updatePasswordRequest is null || !updatePasswordRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidNewPassword);
        }
        var user = await _userRepository.GetByIdAsync(_loggedUser.Id);
        if (user is null)
        {
            throw new AppException(ErrorMessage.CommonMessage);
        }

        var isValidPassword = BCrypt.Net.BCrypt.Verify(updatePasswordRequest.OldPassword, user.Password);

        if (!isValidPassword)
        {
            throw new AppException(ErrorMessage.InvalidPassword);
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordRequest.Password);
        await _userRepository.UpdateAsync(user);
    }

    public async Task<UserResponse> UpdateUser(UpdateUserRequest updateUserRequest)
    {
        if (updateUserRequest is null || !updateUserRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        var updateUser = await _userRepository
            .GetAll()
            .Include(x => x.Company)
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Hash == updateUserRequest.UserHash);

        if (updateUser is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        if (!await VerifyPerrmisonToActionOnUser(updateUser.Company.Hash, PermissionActionEnum.Update))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        updateUser = updateUserRequest.UpdateUser(updateUser);

        await _userRepository.UpdateAsync(updateUser);

        return new UserResponse
        {
            Username = updateUser.Username,
            CompanyHash = updateUser.Company.Hash,
            CompanyName = updateUser.Company.Name,
            Email = updateUser.Email,
            Hash = updateUser.Hash,
            Name = updateUser.Name,
            Surname = updateUser.Surname,
            PhoneNumber = updateUser.PhoneNumber,
            Roles = updateUser.UserRole.Select(x => x.Role.Name).ToList(),
        };
    }

    public async Task<UserResponse> GetUser(Guid userHash)
    {
        var user = await _userRepository
            .GetAll()
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Hash == userHash);

        if (user is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        if (!await VerifyPerrmisonToActionOnUser(user.Company.Hash, PermissionActionEnum.Get) && _loggedUser.Id != user.Id)
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return new UserResponse
        {
            Username = user.Username,
            CompanyHash = user.Company.Hash,
            CompanyName = user.Company.Name,
            Email = user.Email,
            Hash = user.Hash,
            Name = user.Name,
            Surname = user.Surname,
            PhoneNumber = user.PhoneNumber,
            Roles = user.UserRole.Select(x => x.Role.Name).ToList(),
        };
    }

    public async Task FireUser(Guid userHash)
    {
        var user = await _userRepository
            .GetAll()
            .Include(x => x.Company)
            .Include(x => x.UserRole)
            .ThenInclude(y => y.Role)
            .FirstOrDefaultAsync(x => x.Hash == userHash);

        if (user is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        if(user.UserRole.Any(x => x.Role.Name == SystemRoles.Admin))
        {
            throw new AppException(ErrorMessage.CannotModifyAdmin);
        }

        if (!await VerifyPerrmisonToActionOnUser(user.Company.Hash, PermissionActionEnum.Update))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        user.IsFired = true;
        await _userRepository.UpdateAsync(user);
    }

    public async Task<List<UserResponse>> GetUserList()
    {
        var loggedUserCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);

        if (! await VerifyPerrmisonToActionOnUser(loggedUserCompany.Hash, PermissionActionEnum.Get))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return await _userRepository
            .GetAll()
            .Include(x => x.Company)
            .Include(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .Where(x => x.IsFired == false && x.CompanyId == loggedUserCompany.Id)
            .Select(x => x.AsUserReponse())
            .ToListAsync();
    }

    private async Task<bool> VerifyPerrmisonToActionOnUser(Guid targetCompany, PermissionActionEnum action)
    {
        var hasRole = await _roleUtils.HasPermission(new HasPermissionRequest
        {
            Action = action,
            PermissionTo = PermissionToEnum.User,
            Roles = _loggedUser.Roles
        });

        var isUserCompany = true;
        if (action != PermissionActionEnum.Create)
        {
            isUserCompany = await _companyUtils.IsUserCompany(targetCompany, _loggedUser.Id);
        }

        return isUserCompany && hasRole;
    }

    private string GeneratePasssword() =>
         Convert.ToBase64String(RandomNumberGenerator.GetBytes(5)).Replace("=", "");

    private async Task<User> AddUser(User user, Company company, string password)
    {
        user.Hash = Guid.NewGuid();
        user.CompanyId = company.Id;
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        await _userRepository.AddAsync(user);
        return user;
    }

    private Task SendWelcomeMail(User user, string password) =>
        _mailService.SendWelcomeMessage(new WelcomeMessageModel
        {
            Email = user.Email,
            Name = user.Name,
            Password = password,
            Surname = user.Surname,
            Username = user.Username
        });
}
