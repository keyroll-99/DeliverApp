using Microsoft.EntityFrameworkCore;
using Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository._Core
{
    public class BaseHashRepository<T> : BaseRepository<T>, IBaseHashRepository<T>
        where T : BaseHashModel
    {
        public BaseHashRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<T> GetByHashAsync(Guid hash)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Hash == hash);
        }
    }
}
