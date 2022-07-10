using Models.Db;
using Models.Request.Car;
using Models.Response.Car;

namespace Models.Mapper;

public static class CarMapper
{
    public static Car CreateCar(this CreateCarRequest request, long companyId)
        => new()
        {
            Brand = request.Brand,
            Model = request.Model,
            Vin = request.Vin,
            RegistrationNumber = request.RegistrationNumber,
            Hash = Guid.NewGuid(),
            CompanyId = companyId
        };

    public static CarResponse AsResponse (this Car car)
    {
        return new CarResponse
        {
            Driver = car.Driver?.AsBaseUserResponse(),
            Hash = car.Hash,
            RegistrationNumber = car.RegistrationNumber,
            Brand = car.Brand,
            Model = car.Model,
            Vin = car.Vin,
        };
    }

    public static Car UpdateCar(this Car car, UpdateCarRequest updateCar)
    {
        car.Brand = updateCar.Brand;
        car.Model = updateCar.Model;
        car.RegistrationNumber = updateCar.RegistrationNumber;
        car.Vin = updateCar.Vin;
        return car;
    }
}


