namespace Models.Exceptions;

public class ErrorMessage
{
    // todo: specific message shoud be in this same folder where are response
    // here should be only common message
    public const string UserDosentExists = "User doesn't exists";
    public const string UserExists = "User with this username exists";
    public const string UserDosentHaveCompany = "User doesn't have a comapny";
    public const string CompanyDoesntExists = "Company doesn't exists";
    public const string LoggedUserIsNull = "Logged user is null";
    public const string InvalidLoginOrPassword = "Invalid username or password";
    public const string InvalidToken = "Invalid Token";
    public const string TokenAlreadyTaken = "Token already taken";
    public const string InvalidData = "Invalid data";
    public const string InvalidRole = "Invalid Role";
    public const string CommonMessage = "Something went wrong";
    public const string InvalidPassword = "Invalid Password";
    public const string InvalidNewPassword = "The password does not meet the password policy requirements";
}
