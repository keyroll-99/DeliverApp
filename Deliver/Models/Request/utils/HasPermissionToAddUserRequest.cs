namespace Models.Request.utils;

public class HasPermissionToAddUserRequest
{
    public LoggedUser LoggedUser { get; set; }
    public Db.Company LoggedUserCompany { get; set; }
    public Guid TargetCompanyHash { get; set; } 
}
