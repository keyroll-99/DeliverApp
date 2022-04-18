using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Request.Company;
using Models.Response._Core;
using Models.Response.Company;
using Models.Response.User;
using Services.Interface;
using Services.Interface.Utils;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyUtils _companyUtils;
        private readonly LoggedUser _loggedUser;

        public CompanyController(ICompanyService companyService, IOptions<LoggedUser> loggedUser, ICompanyUtils companyUtils)
        {
            _companyService = companyService;
            _loggedUser = loggedUser.Value;
            _companyUtils = companyUtils;
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin)]
        public async Task<BaseRespons<CompanyResponse>> Create(CreateCompanyRequest request)
        {
            return await _companyService.Create(request);
        }

        [HttpGet("Workers")]
        [Authorize(SystemRoles.HR, SystemRoles.CompanyAdmin, SystemRoles.Admin, SystemRoles.CompanyOwner)]
        public async Task<BaseRespons<List<UserResponse>>> GetWorkers()
        {
            var company = await _companyUtils.GetUserCompany(_loggedUser.Id);
            var workers = await _companyService.GetCompanyWorkers(company.Id);

            return workers;
        }
    }
}
