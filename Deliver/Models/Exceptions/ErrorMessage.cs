namespace Models.Exceptions;

public class ErrorMessage
{
    public const string UserDosentExists = "User doesn't exists";
    public const string CompanyDoesntExists = "Company doesn't exists";
    public const string LoggedUserIsNull = "Logged user is null";
    public const string InvalidLoginOrPassword = "Invalid username or password";
    public const string InvalidToken = "Invalid Token";
    public const string TokenAlreadyTaken = "Token already taken";
    public const string InvalidData = "Invalid data";
    public const string InvalidRole = "Invalid Role";
}
