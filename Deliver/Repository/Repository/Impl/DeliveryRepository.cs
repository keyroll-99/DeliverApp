using Microsoft.EntityFrameworkCore;
using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;

namespace Repository.Repository.Impl;

public class DeliveryRepository : BaseHashRepository<Delivery>, IDeliveryRepository
{
    public DeliveryRepository(AppDbContext context) : base(context)
    {
    }

    public override IQueryable<Delivery> GetAll()
    {
        return base.GetAll().Include(x => x.To).Include(y => y.From).Include(x => x.Car);
    }
}
