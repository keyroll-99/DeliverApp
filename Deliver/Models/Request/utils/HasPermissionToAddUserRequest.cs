namespace Models.Request.Utils;

public class HasPermissionToAddUserRequest
{
    public LoggedUser LoggedUser { get; set; }
    public Db.Company LoggedUserCompany { get; set; }
    public Guid TargetCompanyHash { get; set; }
}
