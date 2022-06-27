namespace Models.Request.Account;

public class ChangePasswordRequest
{
    public string Password { get; set; }
    public string OldPassword { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Password)
        && !string.IsNullOrWhiteSpace(OldPassword)
        && Password.Length >= 4;
}
