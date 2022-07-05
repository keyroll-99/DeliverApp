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

    public CarService(ICarRepository carRepository, IRoleUtils roleUtils, IOptions<LoggedUser> loggedUser, ICompanyUtils companyUtils)
    {
        _carRepository = carRepository;
        _roleUtils = roleUtils;
        _loggedUser = loggedUser.Value;
        _companyUtils = companyUtils;
    }

    public Task AssignCarToDeliver(AssignCarToDeliveryRequest request)
    {
        throw new NotImplementedException();
    }

    public Task AssignUserToCar(AssignUserToCarRequest request)
    {
        throw new NotImplementedException();
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

        await _carRepository.AddAsync(request.CreateCar());
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
        var car = await _carRepository.GetAll().FirstOrDefaultAsync(x => x.Hash == hash);

        if(car is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToCarAction(PermissionActionEnum.Get, car))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return car.AsResponse();
    }

    public Task UpdateCar(UpdateCarRequest request)
    {
        throw new NotImplementedException();
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

            isSameCompany = car.Driver.CompanyId == userCompany.Id;
        }

        return hasPermission && isSameCompany;
    }
}


