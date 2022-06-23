using Models.Db.ConstValues;

namespace Models.Response.Authentication;

public class PermissionResponse
{
    public List<PermissionActionEnum> User { get; set; }
    public List<PermissionActionEnum> Location { get; set; }
    public List<PermissionActionEnum> Company { get; set; }
    public List<PermissionActionEnum> Deliver { get; set; }
}
