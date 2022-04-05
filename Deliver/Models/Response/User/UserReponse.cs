namespace Models.Response.User;

public class UserReponse
{
    public Guid Hash { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string CompanyName { get; set; }
    public Guid CompanyHash { get; set; }
}
