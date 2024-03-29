﻿using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Models.Db.ConstValues;
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

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin)]
        public async Task<CompanyResponse> Create(CreateCompanyRequest request)
        {
            return await _companyService.Create(request);
        }

        [HttpGet("List")]
        [Authorize(SystemRoles.Admin)]
        public async Task<List<CompanyResponse>> GetList()
        {
            return await _companyService.GetAllCompany();
        }

        [HttpPut("Assing")]
        [Authorize(SystemRoles.Admin)]
        public async Task<BaseRespons> AssingUserToCompany(AssingUserToCompanyRequest assingUserToCompanyRequest)
        {
            await _companyService.AssingUserToCompany(assingUserToCompanyRequest);
            return BaseRespons.Success();
        }
    }
}
