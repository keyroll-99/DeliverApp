using Models.Db;

namespace Repository.Repository.Interface._Core;

public interface IBaseHashRepository<T> : IBaseRepository<T>
    where T : BaseHashModel
{
    Task<T?> GetByHashAsync(Guid hash);
}
