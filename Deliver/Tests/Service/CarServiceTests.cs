using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using Models;
using Models.Db;
using Models.Exceptions;
using Models.Request.Car;
using Models.Request.Utils.Role;
using NSubstitute;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using Services.Interface.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Service;

public class CarServiceTests
{
    private readonly ICarService _service;

    private readonly ICarRepository _carRepository;
    private readonly IRoleUtils _roleUtils;
    private readonly IOptions<LoggedUser> _loggedUser;
    private readonly ICompanyUtils _companyUtils;
    private readonly IUserUtils _userUtils;
    private readonly IDeliveryUtils _deliveryUtils;

    public CarServiceTests()
    {
        _carRepository = Substitute.For<ICarRepository>();

        _roleUtils = Substitute.For<IRoleUtils>();

        _loggedUser = Substitute.For<IOptions<LoggedUser>>();
        _loggedUser.Value.Returns(new LoggedUser { Id = 1 });

        _companyUtils = Substitute.For<ICompanyUtils>();

        _userUtils = Substitute.For<IUserUtils>();

        _deliveryUtils = Substitute.For<IDeliveryUtils>();

        _service = new CarService(_carRepository, _roleUtils, _loggedUser, _companyUtils, _userUtils, _deliveryUtils);
    }

    [Fact]
    public async Task AssignCarToDeliver_WhenAllDataAreValid_ThenAssingDeliveryToCar()
    {
        // arrange
        _deliveryUtils.GetByHash(Arg.Any<Guid>()).Returns(new Delivery { Id = 1 });
        _carRepository.GetByHashWithPromiseAsync(Arg.Any<Guid>()).Returns(new Car { Id = 1, Delivers = new List<Delivery>() });

        // act
        await _service.AssignCarToDeliver(new AssignCarToDeliveryRequest { CarHash = Guid.NewGuid(), DeliveryHash = Guid.NewGuid() });

        // assert
        await _carRepository.Received(1).UpdateAsync(Arg.Is<Car>(x => x.Id == 1));
    }

    [Fact]
    public async Task AssignUserToCar_WhenDataAreValid_ThenAssingUserToCar()
    {
        // arrange
        _userUtils.GetByHash(Arg.Any<Guid>()).Returns(new User { Id = 1 });
        _carRepository.GetByHashWithPromiseAsync(Arg.Any<Guid>()).Returns(new Car { Id = 1, Delivers = new List<Delivery>() });

        // act
        await _service.AssignUserToCar(new AssignUserToCarRequest { CarHash = Guid.NewGuid(), UserHash = Guid.NewGuid() });

        // assert
        await _carRepository.Received(1).UpdateAsync(Arg.Is<Car>(x => x.Id == 1 && x.DriverId == 1));

    }

    [Fact]
    public async Task CreateCar_WhenReqiestIsInvalid_ThenThrowError()
    {
        // arrange
        var request = new CreateCarRequest
        {
            Brand = string.Empty,
            Model = string.Empty,
            RegistrationNumber = string.Empty,
            Vin = string.Empty
        };

        // act
        Func<Task> act = async () => await _service.CreateCar(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidData);
    }

    [Fact]
    public async Task CreateCar_WhenUserDoesntHavePermission_ThenThrowError()
    {
        // arrange
        var request = new CreateCarRequest
        {
            Brand = "test",
            Model = "test",
            RegistrationNumber = "test",
            Vin = "test"
        };
        _roleUtils.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.CreateCar(request);

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task CreateCar_WhenRequestIsValid_ThenCreateCar()
    {
        // arrange
        var request = new CreateCarRequest
        {
            Brand = "test",
            Model = "test",
            RegistrationNumber = "test",
            Vin = "test"
        };
        _roleUtils.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);
        _companyUtils.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        await _service.CreateCar(request);

        // assert
        await _carRepository.AddAsync(Arg.Any<Car>());
    }

    [Fact]
    public async Task GetAllCars_WhenInvalidPermission_ThenThrowError()
    {
        // arrange
        _roleUtils.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);

        // act
        Func<Task> act = async () => await _service.GetAllCars();

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task GetAllCars_WhenHasPermission_ThenReturnAllPemission()
    {
        // arrnage
        _roleUtils.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);
        _carRepository.GetAll().Returns(new List<Car> { new Car { Id = 1 } }.BuildMock());

        // act
        var result = await _service.GetAllCars();

        // assert
        result.Count.Should().Be(1);
    }

    [Fact]
    public async Task GetCar_WhenUserDoenstHavePermission_ThenThrowError()
    {
        // arrange
        _roleUtils.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(false);
        _carRepository.GetByHashWithPromiseAsync(Arg.Any<Guid>()).Returns(new Car { Id = 1, Driver = new User { CompanyId = 1 } });
        _companyUtils.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        Func<Task> act = async () => await _service.GetCar(Guid.NewGuid());

        // assert
        await act.Should().ThrowAsync<AppException>().WithMessage(ErrorMessage.InvalidRole);
    }

    [Fact]
    public async Task GetCar_WhenUserHasPermission_ThenRetrunCarResponses()
    {
        // arrange
        _roleUtils.HasPermission(Arg.Any<HasPermissionRequest>()).Returns(true);
        _carRepository.GetByHashWithPromiseAsync(Arg.Any<Guid>()).Returns(new Car { Id = 1, Brand = "brand", Driver = new User { CompanyId = 1 }, CompanyId = 1 });
        _companyUtils.GetUserCompany(Arg.Any<long>()).Returns(new Company { Id = 1 });

        // act
        var result = await _service.GetCar(Guid.NewGuid());

        // assert
        result.Should().NotBeNull();
        result.Brand.Should().Be("brand");
    }
}


