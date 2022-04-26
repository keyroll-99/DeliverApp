using Models.Db;
using Models.Request.Company;
using Models.Response.Company;
using Models.Response.User;

namespace Services.Interface;

public interface ICompanyService
{
    Task<CompanyResponse> Create(CreateCompanyRequest request);
    Task<List<UserResponse>> GetCompanyWorkers(long companyId);
}
