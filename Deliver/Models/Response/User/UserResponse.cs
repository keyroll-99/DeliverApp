namespace Models.Response.User;

public class UserResponse : BaseUserResponse
{
    public List<string> Roles { get; set; }
    public string CompanyName { get; set; }
    public Guid CompanyHash { get; set; }
}
