namespace Models.Response.User;

public class BaseUserResponse
{
    public Guid Hash { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}


