using Models.Response.Location;

namespace Models.Response.Deliver;

public class DeliverResponse
{
    public Guid Hash { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public LocationResponse From { get; set; }
    public LocationResponse To { get; set; }
}
