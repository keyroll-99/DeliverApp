using Microsoft.EntityFrameworkCore;
using Models.Db;
using Repository.Repository.Interface._Core;

namespace Repository.Repository.Impl._Core;

public class BaseRepository<T> : IBaseRepository<T>
    where T : BaseModel
{
    protected readonly AppDbContext AppDbContext;
    protected readonly DbSet<T> Entities;

    public BaseRepository(AppDbContext context)
    {
        AppDbContext = context;
        Entities = context.Set<T>();
    }

    public async Task<bool> AddAsync(T model)
    {
        model.CreateTime = DateTime.Now;
        Entities.Add(model);
        return await AppDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(T model)
    {
        Entities.Remove(model);
        return await AppDbContext.SaveChangesAsync() > 0;

    }

    public async Task<bool> DeleteRangeAsync(List<T> models)
    {
        Entities.RemoveRange(models);
        return await AppDbContext.SaveChangesAsync() > 0;
    }

    public virtual IQueryable<T> GetAll()
    {
        return Entities;
    }

    public async Task<T> GetByIdAsync(long id)
    {
        return await Entities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateAsync(T model)
    {
        model.UpdateTime = DateTime.Now;
        Entities.Update(model);
        return await AppDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateRangeAsync(List<T> models)
    {
        foreach (T modelItem in models)
        {
            modelItem.UpdateTime = DateTime.Now;
        }
        Entities.UpdateRange(models);
        return await AppDbContext.SaveChangesAsync() > 0;
    }
}
