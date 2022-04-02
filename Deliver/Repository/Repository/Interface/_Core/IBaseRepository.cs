using Models.Db;

namespace Repository.Repository.Interface._Core;

public interface IBaseRepository<T>
    where T : BaseModel
{
    public Task<bool> AddAsync(T model);
    public Task<T?> GetByIdAsync(long id);
    public Task<bool> UpdateAsync(T model);
    public Task<bool> UpdateRangeAsync(List<T> models);
    public Task<bool> DeleteAsync(T model);
    public Task<bool> DeleteRangeAsync(List<T> models);
    public IQueryable<T> GetAll();
}
