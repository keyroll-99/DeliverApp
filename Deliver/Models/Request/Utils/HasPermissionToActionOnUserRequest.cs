namespace Models.Request.Utils;

public class HasPermissionToActionOnUserRequest
{
    public LoggedUser LoggedUser { get; set; }
    public Db.Company LoggedUserCompany { get; set; }
    public Guid TargetCompanyHash { get; set; }
}
