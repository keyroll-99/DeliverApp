using Models.Db;

namespace Services.Interface.Utils;

public interface IUserUtils
{
    Task<User?> GetById(long id);
    Task<User> GetByHash(Guid hash);
    Task ChangeUserCompany(User user, Company company);
    Task<List<User>> GetUsersFromCompany(Company company);
}
