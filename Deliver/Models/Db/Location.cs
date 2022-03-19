namespace Models.Db;

public class Location : BaseModel
{
    public string Country  { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string? Street { get; set; }
    public string No { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }

    public long CompanyId { get; set; }
    public Company Company { get; set; }
}
    