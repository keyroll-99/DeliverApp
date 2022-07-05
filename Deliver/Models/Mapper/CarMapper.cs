using Models.Db;
using Models.Request.Car;
using Models.Response.Car;

namespace Models.Mapper;

public static class CarMapper
{
    public static Car CreateCar(this CreateCarRequest request)
        => new()
        {
            Brand = request.Brand,
            Model = request.Model,
            Vin = request.Vin,
            RegistrationNumber = request.RegistrationNumber,
            Hash = Guid.NewGuid()
        };

    public static CarResponse AsResponse (this Car car)
    {
        return new CarResponse
        {
            Driver = car.Driver.AsBaseUserResponse(),
            Hash = car.Hash,
            RegistrationNumber = car.RegistrationNumber,
            Brand = car.Brand,
            Model = car.Model,
            Vin = car.Vin,
        };
    }
}


