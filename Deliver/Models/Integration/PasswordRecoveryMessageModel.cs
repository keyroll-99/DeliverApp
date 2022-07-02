namespace Models.Integration;

public class PasswordRecoveryMessageModel
{
    public string Email { get; set; }
    public string RecoveryLink { get; set; }
}
