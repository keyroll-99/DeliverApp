using Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository._Core
{
    public interface IBaseHashRepository<T> : IBaseRepository<T>
        where T : BaseHashModel
    {
        Task<T> GetByHashAsync(Guid hash);
    }
}
