using Models.Db;

namespace Repository.Repository.Interface._Core;

public interface IBaseHashRepository<T> : IBaseRepository<T>
    where T : BaseHashModel
{
    Task<T> GetByHashAsync(Guid hash);
    /// <summary>
    /// Return Enity or throw app exception when not found the entity
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    Task<T> GetByHashWithPromiseAsync(Guid hash);
}
