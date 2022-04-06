using Models.Request.Company;
using Models.Response.Company;

namespace Services.Interface;

public interface ICompanyService
{
    Task<CompanyResponse> Create(CreateCompanyRequest request);
}
