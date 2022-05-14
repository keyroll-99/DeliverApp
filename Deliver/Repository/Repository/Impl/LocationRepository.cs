using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class LocationRepository : BaseHashRepository<Location>, ILocationReposiotry
{
    public LocationRepository(AppDbContext context) : base(context)
    {
    }
}
