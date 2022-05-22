using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Exceptions;
using Models.Request.Location;
using Models.Request.Utils.Role;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace Tests.Service;

public class LocationServiceTest
{
    #region mock
    private readonly LoggedUser _loggedUserMock = new()
    {
        Id = 1,
        Roles = new List<string> { "Admin" }
    };

    private readonly IQueryable<Location> _locationsMock = new List<Location>
    {
        new Location
        {
            Hash = Guid.Parse("634f497f-e7b3-4a81-b50a-6d2b8dc54423"),
            CompanyId = 1,
            Company = new Company
            {
                Users = new List<User> { new User { Id = 1 } }
            }
        },
        new Location
        {
            Hash = Guid.Parse("634f497f-e7b3-4a81-b50a-6d2b8dc54222"),
            CompanyId = 2,
            Company = new Company
            {
                Users = new List<User> { new User { Id = 2 } }
            }
        }
    }.BuildMock();

    #endregion

    private readonly ILocationReposiotry _locationReposiotryMock;
    private readonly IOptions<LoggedUser> _optionsMock;
    private readonly ICompanyUtils _companyUtilsMock;
    private readonly IRoleUtils _roleUtilsMock;
    private readonly ILocationUtils _locationUtils;

    private readonly ILocationService _service;

    public LocationServiceTest()
    {
        _locationReposiotryMock = Substitute.For<ILocationReposiotry>();
        _locationReposiotryMock.GetAll().Returns(_locationsMock);

        _optionsMock = Substitute.For<IOptions<LoggedUser>>();
        _optionsMock.Value.Returns(_loggedUserMock);

        _companyUtilsMock = Substitute.For<ICompanyUtils>();

        _roleUtilsMock = Substitute.For<IRoleUtils>();

        _locationUtils = Substitute.For<ILocationUtils>();

        _service = new LocationService(_locationReposiotryMock, _optionsMock, _companyUtilsMock, _roleUtilsMock, _locationUtils);
    }

    [Fact]
    public async Task CreateLocation_WhenRequestIsInvalid_ThenThrowException()
    {
        // arrange
        var request = new CreateLocationRequest();

        // act
        Func<Task> act = async () => await _service.CreateLocation(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task CreateLocation_WhenUserDoesntHavePermission_ThrowException()
    {
        // arrange
        var request = new CreateLocationRequest
        {
            City = "c",
            Country = "c",
            Email = "c",
            No = "c",
            PhoneNumber = "c",
            PostalCode = "c",
            Region = "c",
            Street = "c"
        };

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.CreateLocation(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task CreateLocation_WhenRequestIsValid_ThenAddNewLocation()
    {
        // arrange
        var request = new CreateLocationRequest
        {
            City = "c",
            Country = "c",
            Email = "c",
            No = "c",
            PhoneNumber = "c",
            PostalCode = "c",
            Region = "c",
            Street = "c"
        };

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company
        {
            Id = 1,
        });

        // act 
        var result = await _service.CreateLocation(request);

        // assert
        result.Should().NotBeNull();
        result.City.Should().Be(request.City);
        result.Country.Should().Be(request.Country);
        result.Street.Should().Be(request.Street);
        result.No.Should().Be(request.No);
        await _locationReposiotryMock.Received(1).AddAsync(Arg.Any<Location>());
    }

    [Fact]
    public async Task DeleteLocation_WhenRequestIsInvalid_ThenThrowException()
    {
        // arrange
        _locationReposiotryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(null as Location);

        // act
        Func<Task> act = async () => await _service.DeleteLocation(Guid.NewGuid());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task DeleteLocation_WhenUserDoesntHavePermission_ThenThrowException()
    {
        // arrange
        _locationReposiotryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new Location { Id = 0, CompanyId = 1 });
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.DeleteLocation(Guid.NewGuid());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task DeleteLocation_WhenUserIsNotInThisSameCompany_ThenThrowException()
    {
        // arrange
        _locationReposiotryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new Location { Id = 0, CompanyId = 1 });
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 2 });
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        // act
        Func<Task> act = async () => await _service.DeleteLocation(Guid.NewGuid());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task DeleteLocation_WhenRequestIsValid_ThenDeleteLocation()
    {
        // arrange
        var request = Guid.NewGuid();
        _locationReposiotryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new Location { Id = 0, CompanyId = 1, Hash = request });
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);


        // act
        await _service.DeleteLocation(request);

        // assert
        await _locationReposiotryMock.Received(1).DeleteAsync(Arg.Is<Location>(x => x.Hash == request));
    }

    [Fact]
    public async Task GetLocationByHash_WhenUserDoesntHavePermission_ThenThrowException()
    {
        // arrange
        _locationUtils.GetLoctaionByHash(Arg.Any<Guid>()).Returns(new Location { Id = 1 });

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);
        
        var request = Guid.Parse("634f497f-e7b3-4a81-b50a-6d2b8dc54423");
        
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        Func<Task> act = async () => await _service.GetLocationByHash(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task GetLocationByHash_WhenRequestIsValid_ThenReturnData()
    {
        var request = Guid.Parse("634f497f-e7b3-4a81-b50a-6d2b8dc54423");

        _locationUtils.GetLoctaionByHash(Arg.Any<Guid>()).Returns(new Location { Id = 1, Hash = request, CompanyId = 1 });

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        var response = await _service.GetLocationByHash(request);

        // assert
        response.Should().NotBeNull();
        response.Hash.Should().Be(request);
    }

    [Fact]
    public async Task GetLocations_WhenUserDoesntHavePermission_ThenThrowError()
    {
        // arrange
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.GetLocations();

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task GetLocations_WhenUserHasPermission_ThenReturnLocationFromUserCompany()
    {
        // arrange
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        // act
        var response = await _service.GetLocations();

        // assert
        response.Count.Should().Be(1);
        response.First().Hash.Should().Be("634f497f-e7b3-4a81-b50a-6d2b8dc54423");
    }

    [Fact]
    public async Task UpdateLocation_WhenRequestIsInvalid_ThenThrowException()
    {
        // arrange
        var request = new UpdateLocationRequest();
    
        // act
        Func<Task> act = async () => await  _service.UpdateLocation(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesntHavePermission_ThenThrowException()
    {
        // arrange
        _locationReposiotryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new Location { CompanyId = 1});
        var request = new UpdateLocationRequest
        {
            City = "c",
            Country = "c",
            Email = "e",
            Hash = Guid.NewGuid(),
            No = "no",
            PhoneNumber = "333",
            PostalCode = "code",
            Region = "region",
            Street = "street"
        };

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);
        
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1});

        // act
        Func<Task> act = async () => await _service.UpdateLocation(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task UpdateUser_WhenRequestIsValid_ThenUpdateLocation()
    {    
        // arrange
        _locationReposiotryMock.GetByHashAsync(Arg.Any<Guid>()).Returns(new Location { CompanyId = 1 });
        var request = new UpdateLocationRequest
        {
            City = "c",
            Country = "c",
            Email = "e",
            Hash = Guid.NewGuid(),
            No = "no",
            PhoneNumber = "333",
            PostalCode = "code",
            Region = "region",
            Street = "street"
        };

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        var result = await _service.UpdateLocation(request);

        // assert
        await _locationReposiotryMock.Received(1).UpdateAsync(Arg.Is<Location>(x =>
            x.City == request.City
            && x.Country == request.Country
            && x.Email == request.Email
            && x.No == request.No
            && x.PhoneNumber == request.PhoneNumber
        ));

        request.Should().BeEquivalentTo(request, o => o.ComparingByValue<UpdateLocationRequest>());
    }
}
