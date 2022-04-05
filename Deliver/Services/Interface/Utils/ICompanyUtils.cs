using Models.Db;

namespace Services.Interface.Utils;

public interface ICompanyUtils
{
    Task<Company?> GetCompanyByHash(Guid hash);
    Task<Company?> GetUserCompany(long userId);
}
