namespace Models.Request.Company;

public class CreateCompanyRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public bool IsValid =>
        Name is not null
        && Email is not null
        && PhoneNumber is not null;
}
