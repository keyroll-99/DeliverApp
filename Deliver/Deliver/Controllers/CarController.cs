using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Car;
using Models.Response.Car;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost("Create")]
        public async Task CreateCar(CreateCarRequest request)
        {
            await _carService.CreateCar(request);
        }

        [HttpPut("Update")]
        public async Task UpdateCar(UpdateCarRequest request)
        {
            await _carService.UpdateCar(request);
        }

        [HttpPut("AssingUserToCar")]
        public async Task AssignUserToCar(AssignUserToCarRequest request)
        {
            await _carService.AssignUserToCar(request);
        }

        [HttpPut("AssingCarToDelivery")]
        public async Task AssignCarToDelivery(AssignCarToDeliveryRequest request)
        {
            await _carService.AssignCarToDeliver(request);
        }

        [HttpGet("{Hash}")]
        public async Task<CarResponse> GetCarByHash(Guid hash)
        {
            return await _carService.GetCar(hash);
        }

        [HttpGet]
        public async Task<List<CarResponse>> GetAllCars()
        {
            return await _carService.GetAllCars();
        }
    }
}