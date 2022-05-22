namespace Models.Request.Deliver;

public class CreateDeliveryRequest
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid FromLocationHash { get; set; }
    public Guid ToLocationHash { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name)
        && FromLocationHash != Guid.Empty
        && ToLocationHash != Guid.Empty
        && StartDate < EndDate;
}
