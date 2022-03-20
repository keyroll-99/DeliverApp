using Models.Db;
using Repository.Repository.Impl._Core;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Impl
{
    public class CarRepository : BaseHashRepository<Car>, ICarRepository
    {
        public CarRepository(AppDbContext context) : base(context)
        {
        }
    }
}
