using Microsoft.EntityFrameworkCore;
using Models.Db;
using Repository.Repository.Interface._Core;

namespace Repository.Repository.Impl._Core;

public class BaseHashRepository<T> : BaseRepository<T>, IBaseHashRepository<T>
    where T : BaseHashModel
{
    public BaseHashRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<T?> GetByHashAsync(Guid hash)
    {
        return await Entities.FirstOrDefaultAsync(x => x.Hash == hash);
    }
}
