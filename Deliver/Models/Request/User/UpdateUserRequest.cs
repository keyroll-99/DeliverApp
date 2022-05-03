namespace Models.Request.User;

public class UpdateUserRequest
{
    public Guid UserHash { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Surname)
        && !string.IsNullOrWhiteSpace(Email)
        && !string.IsNullOrWhiteSpace(Name);

}
