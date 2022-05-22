using Models.Db.ConstValues;

namespace Models.Request.Deliver;

public class ChangeDeliveryStatusRequest
{
    public Guid DeliveryHash { get; set; }
    public DeliveryStatusEnum NewStatus { get; set; }

}
