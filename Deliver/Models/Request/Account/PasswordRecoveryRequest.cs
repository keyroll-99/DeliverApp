namespace Models.Request.Account;

public class PasswordRecoveryRequest
{
    public string Username { get; set; }
    public string Email { get; set; }

    public bool IsValid => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Email);
}
