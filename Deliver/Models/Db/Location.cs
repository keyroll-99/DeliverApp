namespace Models.Db;

public class Location : BaseHashModel
{
    public string Country  { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string No { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public long CompanyId { get; set; }
    public Company Company { get; set; }

    public virtual ICollection<Deliver> Pickup { get; set; } = new List<Deliver>();
    public virtual ICollection<Deliver> Send { get; set; } = new List<Deliver>();
}
    