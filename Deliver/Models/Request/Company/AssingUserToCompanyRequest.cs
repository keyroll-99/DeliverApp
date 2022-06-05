namespace Models.Request.Company;

public class AssingUserToCompanyRequest
{
    public Guid UserHash { get; set; }
    public Guid CompanyHash { get; set; }
}
