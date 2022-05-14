using Models.Db.ConstValues;

namespace Models.Request.Utils.Role;

public class HasPermissionRequest
{
    public List<string> Roles { get; set; }
    public PermissionToEnum PermissionTo { get; set; }
    public PermissionActionEnum Action { get; set; }
}
