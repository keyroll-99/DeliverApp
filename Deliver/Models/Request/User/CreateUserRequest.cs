namespace Models.Request.User;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid CompanyHash { get; set; }
    public string Username { get; set; }
    public List<long> RoleIds { get; set; } = new List<long>();

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name)
        && !string.IsNullOrWhiteSpace(Surname)
        && !string.IsNullOrWhiteSpace(Email)
        && RoleIds.Any()
        && !string.IsNullOrWhiteSpace(Username);
}
