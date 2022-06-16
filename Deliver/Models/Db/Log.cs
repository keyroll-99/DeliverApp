using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Db;

[Table("Logs")]
public class Log : BaseModel
{
    public int LogType { get; set; }
    public string Message { get; set; }
    public string InnerException { get; set; }
    public string StackTrace { get; set; }
}
