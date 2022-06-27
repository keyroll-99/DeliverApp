using Models.Request.Account;

namespace Services.Interface;

public interface IAccountService
{
    Task UpdatePassword(ChangePasswordRequest updatePasswordRequest);
    Task RecoveryPassword(PasswordRecoveryRequest recoveryPasswordRequest);
}
