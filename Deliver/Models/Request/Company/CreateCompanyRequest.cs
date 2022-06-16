namespace Models.Request.Company;

public class CreateCompanyRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name)
        && !string.IsNullOrWhiteSpace(Email)
        && !string.IsNullOrWhiteSpace(PhoneNumber);
}
