namespace Models.Request.Authentication;

public class PasswordRecoveryRequest
{
    public string Username { get; set; }
    public string Email { get; set; }

    public bool isValid => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Email);
}
