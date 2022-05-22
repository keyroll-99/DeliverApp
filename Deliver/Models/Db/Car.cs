namespace Models.Db;

public class Car : BaseHashModel
{
    public string RegistrationNumber { get; set; }

    public long DriverId { get; set; }
    public User Driver { get; set; }

    public virtual ICollection<Delivery> Delivers { get; set; } = new List<Delivery>();
}
