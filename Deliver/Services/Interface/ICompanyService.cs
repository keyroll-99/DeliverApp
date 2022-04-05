using Models.Request.Company;
using Models.Response._Core;
using Models.Response.Company;

namespace Services.Interface;

public interface ICompanyService
{
    Task<BaseRespons<CompanyResponse>> Create(CreateCompanyRequest request);
}
