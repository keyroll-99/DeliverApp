using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Exceptions;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Utils;

public class LocationUtilsTest
{
    #region mock
    private readonly LoggedUser _loggedUserMock = new()
    {
        Id = 1
    };

    private readonly IQueryable<Location> _locationMock = new List<Location>
    {
        new Location
        {
            Hash = Guid.Parse("261ba895-1949-4121-b632-1428072920f3"),
            Id = 1,
            Company = new Company
            {
                Users = new List<User>
                {
                    new User
                    {
                        Id = 1
                    },
                    new User
                    {
                        Id = 2
                    }
                }
            }
        },
        new Location
        {
            Hash = Guid.Parse("2934d08b-4717-42dd-947a-26b0921140ba"),
            Id = 2,
            Company = new Company
            {
                Users = new List<User>
                {
                    new User
                    {
                        Id = 2
                    }
                }
            }
        }
    }.BuildMock();
    #endregion

    private readonly ILocationReposiotry _locationReposiotryMock;
    private readonly IOptions<LoggedUser> _options;
    private readonly LocationUtils _service;

    public LocationUtilsTest()
    {
        _locationReposiotryMock = Substitute.For<ILocationReposiotry>();
        _locationReposiotryMock.GetAll().Returns(_locationMock);

        _options = Substitute.For<IOptions<LoggedUser>>();
        _options.Value.Returns(_loggedUserMock);

        _service = new LocationUtils(_locationReposiotryMock, _options);
    }

    [Fact]
    public async Task GetLocationByHash_WhenLocationExists_ReturnLocation()
    {
        // act
        var response = await _service.GetLoctaionByHash(Guid.Parse("261ba895-1949-4121-b632-1428072920f3"));

        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetLocationByHash_WhenLocationExistsButNotInUserCompany_ThenThrowException()
    {
        // act
        Func<Task> act = async () => await _service.GetLoctaionByHash(Guid.Parse("2934d08b-4717-42dd-947a-26b0921140ba"));

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task GetLocationByHash_WhenLocationDoesnt_ThenThrowException()
    {
        // act
        Func<Task> act = async () => await _service.GetLoctaionByHash(Guid.Parse("2934d08b-1111-42dd-947a-26b0921140ba"));

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }
}
