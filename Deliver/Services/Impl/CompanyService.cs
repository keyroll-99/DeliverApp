using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Request.Company;
using Models.Response._Core;
using Models.Response.Company;
using Repository.Repository.Interface;
using Services.Interface;

namespace Services.Impl;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<BaseRespons<CompanyResponse>> Create(CreateCompanyRequest request)
    {
        if (request is null || !request.IsValid)
        {
            return BaseRespons<CompanyResponse>.Fail("Ivalid Data");
        }

        var company = new Company
        {
            Email = request.Email,
            Hash = Guid.NewGuid(),
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
        };

        var isSuccess = await _companyRepository.AddAsync(company);
        if (!isSuccess)
        {
            return BaseRespons<CompanyResponse>.Fail("Something went wrong");
        }

        return new CompanyResponse
        {
            Email = company.Email,
            Hash = company.Hash,
            Name = company.Name,
            PhoneNumber = company.PhoneNumber
        };
    }
}