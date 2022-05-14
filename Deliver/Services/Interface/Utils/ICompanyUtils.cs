using Models.Db;

namespace Services.Interface.Utils;

public interface ICompanyUtils
{
    Task<Company?> GetCompanyByHash(Guid hash);
    Task<Company> GetUserCompany(long userId);
    Task<bool> IsUserCompany(Guid companyHash, long userId);
}
