using Models.Db;

namespace Services.Interface.Utils;

public interface IDeliveryUtils
{
    Task<Delivery> GetByHash(Guid hash);
}


