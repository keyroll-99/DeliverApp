using Models.Db;

namespace Services.Interface.Utils;

public interface ILocationUtils
{
    Task<Location> GetLoctaionByHash(Guid hash);
}
