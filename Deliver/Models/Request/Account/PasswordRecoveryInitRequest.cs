namespace Models.Request.Account;

public class PasswordRecoveryInitRequest
{
    public string Username { get; set; }
    public string Email { get; set; }

    public bool IsValid => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Email);
}
