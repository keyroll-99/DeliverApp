namespace Models.Db;

public class Deliver : BaseHashModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    
    public long CarId { get; set; }
    public Car Car { get; set; }

    public long FromId { get; set; }
    public Location From { get; set; }

    public long ToId { get; set; }
    public Location To { get; set; }
}
