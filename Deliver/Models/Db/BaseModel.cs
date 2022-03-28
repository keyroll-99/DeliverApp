using System.ComponentModel.DataAnnotations;

namespace Models.Db;

public class BaseModel
{
    [Key]
    public long Id { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}
