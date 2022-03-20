using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class DeliverRepository : BaseHashRepository<Deliver>, IDeliverRepository
{
    public DeliverRepository(AppDbContext context) : base(context)
    {
    }
}
