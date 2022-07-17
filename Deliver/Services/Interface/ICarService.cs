using Models.Request.Car;
using Models.Response.Car;

namespace Services.Interface;

public interface ICarService
{
    Task CreateCar(CreateCarRequest request);
    Task UpdateCar(UpdateCarRequest request);
    Task<CarResponse> GetCar(Guid hash);
    Task<List<CarResponse>> GetAllCars();
    Task AssignUserToCar(AssignUserToCarRequest request);
    Task AssignCarToDeliver(AssignCarToDeliveryRequest request);
}

