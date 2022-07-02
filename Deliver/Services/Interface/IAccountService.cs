using Models.Request.Account;

namespace Services.Interface;

public interface IAccountService
{
    Task UpdatePassword(ChangePasswordRequest updatePasswordRequest);
    Task SetNewPassword(PasswordRecoverySetNewPasswordRequest newPasswordRecoverySetNewPasswordRequest);
    Task InitRecoveryPassword(PasswordRecoveryInitRequest recoveryPasswordRequest);
    Task<bool> IsValidRecoveryKey(string recoveryKey);
}
