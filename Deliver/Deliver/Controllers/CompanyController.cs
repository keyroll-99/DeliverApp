using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Request.Company;
using Models.Response._Core;
using Models.Response.Company;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin)]
        public async Task<BaseRespons<CompanyResponse>> Create(CreateCompanyRequest request)
        {
            return await _companyService.Create(request);
        }
    }
}
