﻿using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Exceptions;
using Models.Request.Company;
using Models.Response.Company;
using Models.Response.User;
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

    public async Task<List<UserResponse>> GetCompanyWorkers(long companyId)
    {
        var company = await _companyRepository
            .GetAll()
            .Include(x => x.Users)
            .ThenInclude(x => x.UserRole)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == companyId);

        if (company is null)
        {
            throw new AppException(ErrorMessage.CompanyDoesntExists);
        }

        var users = company
            .Users.
            Select(x => new UserResponse
            {
                CompanyHash = company.Hash,
                CompanyName = company.Name,
                Email = x.Email,
                Hash = x.Hash,
                Name = x.Name,
                Surname = x.Surname,
                Username = x.Username,
                PhoneNumber = x.PhoneNumber,
                Roles = x.UserRole.Select(x => x.Role.Name).ToList()
            }).ToList();

        return users;
    }
}