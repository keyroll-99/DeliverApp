namespace Models.Response.Location;

public class LocationResponse
{
    public Guid Hash { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string No { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
