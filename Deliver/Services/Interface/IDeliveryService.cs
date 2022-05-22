using Models.Request.Deliver;
using Models.Response.Deliver;

namespace Services.Interface;

public interface IDeliveryService
{
    Task<DeliverResponse> CreateDelivery(CreateDeliveryRequest createDeliverRequest);
    Task<List<DeliverResponse>> GetAllDeliveries();
    Task<DeliverResponse> GetDeliveryByHash(Guid hash);
    Task<DeliverResponse> ChangeStatus(ChangeDeliveryStatusRequest changeDeliverStatusRequest);
    Task<DeliverResponse> UpdateDelivery(UpdateDeliveryRequest updateDeliverRequest);
}
