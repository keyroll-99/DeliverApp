using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Request.Deliver;
using Models.Request.Utils.Role;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service;

public class DeliveryServiceTest
{
    #region mock
    private readonly LoggedUser _loggedUser = new()
    {
        Id = 1,
        Roles = new List<string> { "role1" }
    };

    private readonly IQueryable<Delivery> _deliversMock = new List<Delivery>{
        new Delivery {
            Name = "test-1",
            Hash = Guid.NewGuid(),
            From = new Location
            {
                Company = new Company
                {
                    Id = 1,
                    Users = new List<User>
                    {
                        new User
                        {
                            Id = 1
                        }
                    }
                },
                CompanyId = 1,
            },
            To = new Location
            {
                Company = new Company
                {
                    Id = 2,
                    Users = new List<User>
                    {
                        new User
                        {
                            Id = 1
                        }
                    }
                },
                CompanyId = 2,
            }
        },
        new Delivery {
            Name = "test-3",
            Hash= Guid.NewGuid(),
            From = new Location
            {
                Company = new Company
                {
                    Id = 3,
                    Users = new List<User>
                    {
                        new User
                        {
                            Id = 2
                        }
                    }
                }
            },
            To = new Location
            {
                Company = new Company
                {
                    Users = new List<User>
                    {
                        new User
                        {
                            Id = 1
                        }
                    }
                }
            }
        },
        new Delivery {
            Name = "test-2",
            Hash = Guid.NewGuid(),
            From = new Location
            {
                Company = new Company
                {
                    Users = new List<User>
                    {
                        new User
                        {
                            Id = 1
                        }
                    }
                }
            },
            To = new Location
            {
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
        }
    }.BuildMock();
    #endregion

    private readonly ILocationUtils _locationUtilsMock;
    private readonly IRoleUtils _roleUtilsMock;
    private readonly ICompanyUtils _companyUtilsMock;
    private readonly IDeliveryRepository _deliverRepositoryMock;
    private readonly IOptions<LoggedUser> _loggedUserOptionsMock;

    private readonly DeliveryService _service;

    public DeliveryServiceTest()
    {
        _locationUtilsMock = Substitute.For<ILocationUtils>();

        _roleUtilsMock = Substitute.For<IRoleUtils>();

        _companyUtilsMock = Substitute.For<ICompanyUtils>();

        _deliverRepositoryMock = Substitute.For<IDeliveryRepository>();
        _deliverRepositoryMock.GetAll().Returns(_deliversMock);

        _loggedUserOptionsMock = Substitute.For<IOptions<LoggedUser>>();
        _loggedUserOptionsMock.Value.Returns(_loggedUser);

        _service = new DeliveryService(_locationUtilsMock, _roleUtilsMock, _companyUtilsMock, _deliverRepositoryMock, _loggedUserOptionsMock);
    }

    [Fact]
    public async Task CreateDeliver_WhenRequestIsInvalid_ThenThrowError()
    {
        // arrange
        var request = new CreateDeliveryRequest();

        // act
        Func<Task> act = async () => await _service.CreateDelivery(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task CreateDeliver_WhenUserDoesntHavePermission_ThenThrowError()
    {
        // arrange
        var request = new CreateDeliveryRequest
        {
            EndDate = DateTime.Now.AddDays(1),
            FromLocationHash = Guid.NewGuid(),
            Name = "name",
            StartDate = DateTime.Now,
            ToLocationHash = Guid.NewGuid(),
        };

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.CreateDelivery(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task CreateDeliver_WhenRequestIsValid_ThenAddNewDeliver()
    {
        // arrange
        var request = new CreateDeliveryRequest
        {
            EndDate = DateTime.Now.AddDays(1),
            FromLocationHash = Guid.NewGuid(),
            Name = "name",
            StartDate = DateTime.Now,
            ToLocationHash = Guid.NewGuid(),
        };

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        _locationUtilsMock.GetLoctaionByHash(Arg.Any<Guid>()).Returns(new Location { Id = 1 });

        // act
        var response = await _service.CreateDelivery(request);

        // assert
        response.EndDate = request.EndDate;
        response.Name = request.Name;
        response.StartDate = request.StartDate;

        await _deliverRepositoryMock
            .Received(1)
            .AddAsync(Arg.Is<Delivery>(x =>
                x.EndDate == request.EndDate
                && x.Name == request.Name
                && x.StartDate == request.StartDate
            ));
    }

    [Fact]
    public async Task GetAllDelivers_WhenUserDoesntHavePermission_ThenThrowException()
    {
        // arrange
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.GetAllDeliveries();

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task GetAllDelivers_WhenUserHasPermission_ThenReturnAllDeliverFromCompany()
    {
        // arrange
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        // act
        var result = await _service.GetAllDeliveries();

        // assert
        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task ChangeStatus_WhenDeliveryDoesntExist_ThenThrowError()
    {
        // arrange
        var request = new ChangeDeliveryStatusRequest
        {
            DeliveryHash = Guid.NewGuid(),
            NewStatus = DeliveryStatusEnum.New
        };


        // act
        Func<Task> act = async () => await _service.ChangeStatus(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task ChagneStatus_WhenUserDoesntHavePermission_ThenThrowError()
    {
        // arrange
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        var request = new ChangeDeliveryStatusRequest
        {
            DeliveryHash = _deliversMock.First().Hash,
            NewStatus = DeliveryStatusEnum.New,
        };

        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        Func<Task> act = async () => await _service.ChangeStatus(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task ChangeStatus_WhenRequestIsValid_ThenChangeStatusAndReturnDelivery()
    {
        // arrange
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        var request = new ChangeDeliveryStatusRequest
        {
            DeliveryHash = _deliversMock.First().Hash,
            NewStatus = DeliveryStatusEnum.New,
        };

        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        var result = await _service.ChangeStatus(request);

        // assert
        result.Status.Should().Be(((int)request.NewStatus));

        await _deliverRepositoryMock.Received(1).UpdateAsync(Arg.Is<Delivery>(x => x.Status == ((int)DeliveryStatusEnum.New)));
    }

    [Fact]
    public async Task GetDeliveryByHash_WhenDeliveryDoesntExists_ThenThrowError()
    {
        // act
        Func<Task> act = async () => await _service.GetDeliveryByHash(Guid.NewGuid());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task GetDeliveryByHash_WhenDoesntHavePermission_ThenThrowError()
    {
        // arrange
        Guid hash = (await _deliverRepositoryMock.GetAll().FirstAsync()).Hash;
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });


        // act
        Func<Task> act = async () => await _service.GetDeliveryByHash(hash);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task GetDeliverByHash_WhenValidRequest_ThenReturnDelivery()
    {
        // arrange
        var delivery = (await _deliverRepositoryMock.GetAll().FirstAsync());
        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });


        // act
        var response = await _service.GetDeliveryByHash(delivery.Hash);

        // assert
        response.EndDate.Should().Be(delivery.EndDate);
        response.Name.Should().Be(delivery.Name);

    }

    [Fact]
    public async Task UpdateDelivery_WhenRequestIsInvalid_ThenThrowError()
    {
        // act
        Func<Task> act = async () => await _service.UpdateDelivery(new UpdateDeliveryRequest());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task UpdateDelivery_WhenDeliverDoesntExist_ThenThrowError()
    {
        // arrange
        var request = new UpdateDeliveryRequest
        {
            EndDate = DateTime.Now.AddDays(1),
            FromLocationHash = Guid.NewGuid(),
            Name = "name",
            DeliveryHash = Guid.NewGuid(),
            StartDate = DateTime.Now,
            ToLocationHash = Guid.NewGuid()
        };

        // act
        Func<Task> act = async () => await _service.UpdateDelivery(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task UpdateDelivery_WhenUserDoesnHavePermission_ThenThrowError()
    {
        // arrange
        var request = new UpdateDeliveryRequest
        {
            EndDate = DateTime.Now.AddDays(1),
            FromLocationHash = Guid.NewGuid(),
            Name = "name",
            DeliveryHash = (await _deliverRepositoryMock.GetAll().FirstAsync()).Hash,
            StartDate = DateTime.Now,
            ToLocationHash = Guid.NewGuid()
        };
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.UpdateDelivery(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task UpdateDelivery_WhenRequestIsValid_ThenUpdateDelivery()
    {
        // arrange
        var delivery = await _deliverRepositoryMock.GetAll().FirstAsync();
        var request = new UpdateDeliveryRequest
        {
            EndDate = DateTime.Now.AddDays(1),
            FromLocationHash = Guid.NewGuid(),
            Name = "name",
            DeliveryHash = delivery.Hash,
            StartDate = DateTime.Now,
            ToLocationHash = Guid.NewGuid()
        };
        _companyUtilsMock.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        _roleUtilsMock.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);

        _locationUtilsMock.GetLoctaionByHash(Arg.Any<Guid>()).Returns(new Location { Id = 1 });

        // act
        var response = await _service.UpdateDelivery(request);

        // assert
        await _deliverRepositoryMock.Received(1).UpdateAsync(Arg.Is<Delivery>(x =>
            x.Hash == delivery.Hash
            && x.EndDate == request.EndDate
            && x.Name == request.Name
            && x.StartDate == request.StartDate
        ));
    }
}
