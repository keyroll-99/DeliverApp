namespace Models.Db;

public class BaseModel
{
    public long Id { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
