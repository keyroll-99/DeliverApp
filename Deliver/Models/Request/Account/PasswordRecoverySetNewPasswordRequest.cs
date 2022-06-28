namespace Models.Request.Account;

public class PasswordRecoverySetNewPasswordRequest
{
    public string NewPassword { get; set; }
    public string RecoveryKey { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(RecoveryKey)
        && !string.IsNullOrWhiteSpace(NewPassword)
        && NewPassword.Length > 4;
}
