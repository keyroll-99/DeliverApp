namespace Models.Request.Car;

public class AssignCarToDeliveryRequest
{
    public Guid CarHash { get; set; }
    public Guid DeliveryHash { get; set; }
}


