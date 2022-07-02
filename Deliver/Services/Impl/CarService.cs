using Repository.Repository.Interface;
using Services.Interface;

namespace Services.Impl;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }
}


