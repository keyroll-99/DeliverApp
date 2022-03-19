namespace Models.Db;

public class Company : BaseModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }

    public virtual ICollection<User> Users { get; set; }
    public virtual ICollection<Location> Locations { get; set; }
}
