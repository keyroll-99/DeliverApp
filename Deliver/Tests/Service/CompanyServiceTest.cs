using FluentAssertions;
using MockQueryable.NSubstitute;
using Models.Db;
using Models.Exceptions;
using Models.Request.Company;
using Models.Response.Company;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Utils;

public class CompanyServiceTest
{
    #region mock
    private readonly IQueryable<Company> _companiesMock = new List<Company>
    {
        new Company
        {
            Id = 1,
            Hash = Guid.NewGuid(),
            Name = "name",
            Users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "test1@",
                    Hash = Guid.NewGuid(),
                    Name = "name",
                    Surname = "surname",
                    CompanyId = 1,
                    UserRole = new List<UserRole>
                    {
                        new UserRole
                        {
                            Role = new Role
                            {
                                Name = "admin"
                            }
                        }
                    }
                },
                new User
                {
                    Id = 2,
                    Email = "test2@",
                    Hash = Guid.NewGuid(),
                    Name = "name2",
                    Surname = "surname2",
                    CompanyId = 1,
                    UserRole = new List<UserRole>
                    {
                        new UserRole
                        {
                            Role = new Role
                            {
                                Name = "admin"
                            }
                        }
                    }
                }
            }
        },
        new Company
        {
            Id = 1,
            Hash = Guid.NewGuid(),
            Name = "name",
            Users = new List<User>
            {
                new User
                {
                    Id = 3,
                    Email = "test1@",
                    Hash = Guid.NewGuid(),
                    Name = "name",
                    Surname = "surname",
                    CompanyId= 2,
                    UserRole = new List<UserRole>
                    {
                        new UserRole
                        {
                            Role = new Role
                            {
                                Name = "admin"
                            }
                        }
                    }
                },
                new User
                {
                    Id = 4,
                    Email = "test2@",
                    Hash = Guid.NewGuid(),
                    Name = "name2",
                    Surname = "surname2",
                    CompanyId = 2,
                    UserRole = new List<UserRole>
                    {
                        new UserRole
                        {
                            Role = new Role
                            {
                                Name = "admin"
                            }
                        }
                    }
                }
            }
        }
    }.BuildMock();
    #endregion

    private readonly ICompanyRepository _companyRepository;
    private readonly CompanyService _service;

    public CompanyServiceTest()
    {
        _companyRepository = Substitute.For<ICompanyRepository>();
        _companyRepository.GetAll().Returns(_companiesMock);
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

    [Fact]
    public async Task GetCompanyWorkers_WhenCompanyDoesntExists_ThrowException()
    {
        // act
        Func<Task> act = async () => await _service.GetCompanyWorkers(4);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.CompanyDoesntExists);
    }

    [Fact]
    public async Task GetCompanyWorkers_WhenCompanyExists_ThenReturnUsersFormCompany()
    {
        // act
        var response = await _service.GetCompanyWorkers(1);
        
        // assert
        response.Count.Should().Be(2);
    }
}
