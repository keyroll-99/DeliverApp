using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Mapper;
using Models.Request.Car;
using Models.Request.Utils.Role;
using Models.Response.Car;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;

namespace Services.Impl;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IRoleUtils _roleUtils;
    private readonly LoggedUser _loggedUser;
    private readonly ICompanyUtils _companyUtils;
    private readonly IUserUtils _userUtils;
    private readonly IDeliveryUtils _deliveryUtils;

    public CarService(
        ICarRepository carRepository,
        IRoleUtils roleUtils,
        IOptions<LoggedUser> loggedUser,
        ICompanyUtils companyUtils,
        IUserUtils userUtils,
        IDeliveryUtils deliveryUtils
        )
    {
        _carRepository = carRepository;
        _roleUtils = roleUtils;
        _loggedUser = loggedUser.Value;
        _companyUtils = companyUtils;
        _deliveryUtils = deliveryUtils;
        _userUtils = userUtils;
    }

    public async Task AssignCarToDeliver(AssignCarToDeliveryRequest request)
    {
        var delivery = await _deliveryUtils.GetByHash(request.DeliveryHash);
        var car = await _carRepository.GetByHashWithPromiseAsync(request.CarHash);

        car.Delivers.Add(delivery);
        await _carRepository.UpdateAsync(car);
    }

    public async Task AssignUserToCar(AssignUserToCarRequest request)
    {
        var car = await _carRepository.GetByHashWithPromiseAsync(request.CarHash);
        var user = await _userUtils.GetByHash(request.UserHash);

        car.Driver = user;
        car.DriverId = user.Id;

        await _carRepository.UpdateAsync(car);
    }

    public async Task CreateCar(CreateCarRequest request)
    {
        if (request is null || !request.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToCarAction(PermissionActionEnum.Create))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }
        var userCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);

        await _carRepository.AddAsync(request.CreateCar(userCompany.Id));
    }

    public async Task<List<CarResponse>> GetAllCars()
    {
        if (!await HasPermissionToCarAction(PermissionActionEnum.Get))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return await _carRepository.GetAll().Select(x => x.AsResponse()).ToListAsync();
    }

    public async Task<CarResponse> GetCar(Guid hash)
    {
        var car = await _carRepository.GetByHashWithPromiseAsync(hash);

        if (!await HasPermissionToCarAction(PermissionActionEnum.Get, car))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return car.AsResponse();
    }

    public async Task UpdateCar(UpdateCarRequest request)
    {
        var car = await _carRepository.GetByHashWithPromiseAsync(request.Hash);

        if (!await HasPermissionToCarAction(PermissionActionEnum.Update, car))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        car = car.UpdateCar(request);

        await _carRepository.UpdateAsync(car);
    }

    private async Task<bool> HasPermissionToCarAction(PermissionActionEnum action, Car? car = null)
    {
        var hasPermission = await _roleUtils.HasPermission(new HasPermissionRequest
        {
            Action = action,
            PermissionTo = PermissionToEnum.Car,
            Roles = _loggedUser.Roles
        });

        var isSameCompany = true;
        if (car is not null)
        {
            var userCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);

            isSameCompany = car.CompanyId == userCompany.Id;
        }

        return hasPermission && isSameCompany;
    }
}


