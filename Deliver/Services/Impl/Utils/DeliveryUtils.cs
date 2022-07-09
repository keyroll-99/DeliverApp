using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Exceptions;
using Repository.Repository.Interface;
using Services.Interface.Utils;

namespace Services.Impl.Utils;

public class DeliveryUtils : IDeliveryUtils
{
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliveryUtils(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public async Task<Delivery> GetByHash(Guid hash)
    {
        var delivery = await _deliveryRepository.GetAll().FirstOrDefaultAsync(x => x.Hash == hash);
        if(delivery is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        return delivery;
    }
}


