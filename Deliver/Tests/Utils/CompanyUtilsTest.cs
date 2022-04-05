using FluentAssertions;
using MockQueryable.NSubstitute;
using Models.Db;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl.Utils;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Utils;

public class CompanyUtilsTest
{
    #region mock

    private readonly IQueryable<Company> _companyDataMock = new List<Company>
    {
        new Company
        {
            Hash = Guid.Parse("9122a83e-793f-4277-a33a-51eac917aef6"),
            Users = new List<User>
            {
                new User
                {
                    Id = 1,
                }
            }
        }
    }.BuildMock();

    #endregion

    private readonly ICompanyRepository _companyRepositoryMock;
    private readonly ICompanyUtils _service;

    public CompanyUtilsTest()
    {
        _companyRepositoryMock = Substitute.For<ICompanyRepository>();
        _companyRepositoryMock.GetAll().Returns(_companyDataMock);

        _service = new CompanyUtils(_companyRepositoryMock);
    }

    [Fact]
    public async Task GetCompanyByHash_WhenCompanyExists_ReturnCompany()
    {
        // arrange
        var guid = Guid.Parse("9122a83e-793f-4277-a33a-51eac917aef6");

        // act
        var result = await _service.GetCompanyByHash(guid);

        // assert
        result.Should().NotBeNull();
        result.Hash.Should().Be(guid);
    }

    [Fact]
    public async Task GetUserCompany_WhenUserHaveCompany_ReturnCompany()
    {
        // act
        var result = await _service.GetUserCompany(1);

        // assert
        result.Should().NotBeNull();
        result.Users.Should().Contain(x => x.Id == 1);
    }
}
