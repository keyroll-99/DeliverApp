using Models.Db;

namespace Services.Interface.Utils;

public interface IUserUtils
{
    Task<User?> GetById(long id);
    Task<List<User>> GetUsersFromCompany(Company company);
}
