using Models.Db.ConstValues;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Db;

public class RolePermission : BaseModel
{
    public long RoleId { get; set; }
    public Role Role { get; set; }

    [Column("PermissionToId")]
    public PermissionToEnum PermissionTo { get; set; }

    [Column("PermissionActionId")]
    public PermissionActionEnum PermissionAction { get; set; }

}
