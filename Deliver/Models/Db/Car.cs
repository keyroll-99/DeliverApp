namespace Models.Db;

public class Car : BaseHashModel
{
    public string RegistrationNumber { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Vin { get; set; }

    public long DriverId { get; set; }
    public User Driver { get; set; }

    public virtual ICollection<Delivery> Delivers { get; set; } = new List<Delivery>();
}
