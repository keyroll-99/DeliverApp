using Models.Db;
using Models.Db.ConstValues;
using Models.Request.Deliver;
using Models.Response.Deliver;

namespace Models.Mapper;

public static class DeliveryMapper
{
    public static DeliverResponse AsResponse(this Delivery deliver)
        => new()
        {
            Hash = deliver.Hash,
            Name = deliver.Name,
            StartDate = deliver.StartDate,
            EndDate = deliver.EndDate,
            Status = deliver.Status,
            From = deliver.From.AsLocationResponse(),
            To = deliver.To.AsLocationResponse(),
            Car = deliver.Car?.AsResponse(),
        };

    public static Delivery CreateDelivery(this CreateDeliveryRequest createDeliveryRequest)
        => new()
        {
            Name = createDeliveryRequest.Name,
            Status = ((int)DeliveryStatusEnum.New),
            StartDate = createDeliveryRequest.StartDate,
            EndDate = createDeliveryRequest.EndDate,
            Hash = Guid.NewGuid(),
            Car = null,
            CarId = null,
        };

    public static Delivery UpdateDelivery(this Delivery delivery, UpdateDeliveryRequest updateDeliveryRequest)
    {
        delivery.EndDate = updateDeliveryRequest.EndDate;
        delivery.StartDate = updateDeliveryRequest.StartDate;
        delivery.Name = updateDeliveryRequest.Name;

        return delivery;
    }
}
