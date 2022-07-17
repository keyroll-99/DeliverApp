using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Request.Car;
using Models.Response.Car;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin)]
        public async Task CreateCar(CreateCarRequest request)
        {
            await _carService.CreateCar(request);
        }

        [HttpPut("Update")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin)]
        public async Task UpdateCar(UpdateCarRequest request)
        {
            await _carService.UpdateCar(request);
        }

        [HttpPut("AssingUserToCar")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin)]
        public async Task AssignUserToCar(AssignUserToCarRequest request)
        {
            await _carService.AssignUserToCar(request);
        }

        [HttpPut("AssingCarToDelivery")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin)]
        public async Task AssignCarToDelivery(AssignCarToDeliveryRequest request)
        {
            await _carService.AssignCarToDeliver(request);
        }

        [HttpGet("{Hash}")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin, SystemRoles.Driver, SystemRoles.Dispatcher)]
        public async Task<CarResponse> GetCarByHash(Guid hash)
        {
            return await _carService.GetCar(hash);
        }

        [HttpGet]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin, SystemRoles.Driver, SystemRoles.Dispatcher)]
        public async Task<List<CarResponse>> GetAllCars()
        {
            return await _carService.GetAllCars();
        }
    }
}