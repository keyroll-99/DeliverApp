using Integrations.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Exceptions;
using Models.Integration;
using Models.Request.Account;
using Repository.Repository.Interface;
using Services.Interface;

namespace Services.Impl;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
    private readonly IMailService _mailService;
    private readonly LoggedUser _loggedUser;
    private const int _howLongRecoveryKeyIsValidInMinutes = 30;

    public AccountService(
        IUserRepository userRepository,
        IOptions<LoggedUser> loggedUser,
        IPasswordRecoveryRepository passwordRecoveryRepository,
        IMailService mailService)
    {
        _userRepository = userRepository;
        _loggedUser = loggedUser.Value;
        _passwordRecoveryRepository = passwordRecoveryRepository;
        _mailService = mailService;
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

    public async Task InitRecoveryPassword(PasswordRecoveryInitRequest passwordRecoveryRequest)
    {
        var passwordRecoveryModel = await GeneratePasswordRecoveryModel(passwordRecoveryRequest);
        var addTask = _passwordRecoveryRepository.AddAsync(passwordRecoveryModel);
        var sendMailTask = _mailService.SendPasswordRecoveryMessage(new PasswordRecoveryMessageModel { Email = passwordRecoveryRequest.Email, RecoveryLink = passwordRecoveryModel.Hash.ToString() });

        Task.WaitAll(addTask, sendMailTask);
    }

    public async Task SetNewPassword(PasswordRecoverySetNewPasswordRequest newPasswordRecoverySetNewPasswordRequest)
    {
        if(newPasswordRecoverySetNewPasswordRequest is null || !newPasswordRecoverySetNewPasswordRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        var user = await _userRepository
            .GetAll()
            .Include(x => x.PasswordRecoveries)
            .FirstOrDefaultAsync(x => x.PasswordRecoveries.Any(x => x.Hash.ToString() == newPasswordRecoverySetNewPasswordRequest.RecoveryKey && x.CreateTime > DateTime.Now.AddMinutes(-_howLongRecoveryKeyIsValidInMinutes)));

        if(user is null)
        {
            throw new AppException(ErrorMessage.TokenExpiredOrInvalidPasswordRecoveryLink);
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPasswordRecoverySetNewPasswordRequest.NewPassword);

        await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> IsValidRecoveryKey(string recoveryKey)
    {
        return await _passwordRecoveryRepository.GetAll().AnyAsync(x => x.Hash.ToString() == recoveryKey && x.CreateTime > DateTime.Now.AddMinutes(-_howLongRecoveryKeyIsValidInMinutes));
    }

    private async Task<PasswordRecovery> GeneratePasswordRecoveryModel(PasswordRecoveryInitRequest passwordRecoveryRequest)
    {
        if (passwordRecoveryRequest is null || !passwordRecoveryRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        var user = await _userRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.Username == passwordRecoveryRequest.Username && x.Email == passwordRecoveryRequest.Email);

        if (user is null)
        {
            throw new AppException(ErrorMessage.UserDosentExists);
        }

        var recoveryLink = Guid.NewGuid();

        return new PasswordRecovery
        {
            Hash = recoveryLink,
            User = user,
            UserId = user.Id,
        };
    }
}
