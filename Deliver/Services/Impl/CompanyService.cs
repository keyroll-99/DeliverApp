using Models.Db;
using Models.Exceptions;
using Models.Request.Company;
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

    public async Task<CompanyResponse> Create(CreateCompanyRequest request)
    {
        if (request is null || !request.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        var company = new Company
        {
            Email = request.Email,
            Hash = Guid.NewGuid(),
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
        };

        await _companyRepository.AddAsync(company);

        return new CompanyResponse
        {
            Email = company.Email,
            Hash = company.Hash,
            Name = company.Name,
            PhoneNumber = company.PhoneNumber
        };
    }
}