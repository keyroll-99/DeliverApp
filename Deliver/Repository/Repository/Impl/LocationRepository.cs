using Models.Db;
using Repository.Repository.Impl._Core;

namespace Repository.Repository.Impl;

public class LocationRepository : BaseHashRepository<Deliver>, ILocationRepository
{
    public LocationRepository(AppDbContext context) : base(context)
    {
    }
}
