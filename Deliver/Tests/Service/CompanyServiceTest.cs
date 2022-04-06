using FluentAssertions;
using Models.Db;
using Models.Exceptions;
using Models.Request.Company;
using Models.Response.Company;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Utils;

public class CompanyServiceTest
{
    private readonly ICompanyRepository _companyRepository;
    private readonly CompanyService _service;

    public CompanyServiceTest()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(true);

        _service = new CompanyService(_companyRepository);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("test", "test@test.com", null)]
    [InlineData("test", null, "111-111-111")]
    [InlineData(null, "test@test.com", "111-111-111")]
    public async Task Create_WhenRequestIsInvalid_ThenReturnError(string? name, string? email, string? phoneNumber)
    {
        // arrange
        var request = new CreateCompanyRequest
        {
            Email = email,
            PhoneNumber = phoneNumber,
            Name = name,
        };

        // act
        Func<Task> act = async () => await _service.Create(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ThenReturnSuccess()
    {
        // arrange
        var request = new CreateCompanyRequest
        {
            Name = "test",
            PhoneNumber = "111-111-111",
            Email = "test@test.com"
        };


        // act
        var response = await _service.Create(request);

        // assert
        var expectetCompanyResponse = new CompanyResponse
        {
            Email = request.Email,
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
        };

        response.Should().NotBeNull(); ;
        response.Should().BeEquivalentTo(expectetCompanyResponse, o => o.Excluding(x => x.Hash));
    }
}
