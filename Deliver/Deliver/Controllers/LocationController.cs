using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Request.Location;
using Models.Response._Core;
using Models.Response.Location;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize(SystemRoles.Admin, SystemRoles.CompanyOwner, SystemRoles.CompanyAdmin)]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("List")]
        public async Task<BaseRespons<List<LocationResponse>>> GetAll()
        {
            return await _locationService.GetLocations();
        }

        [HttpPost]
        public async Task<BaseRespons<LocationResponse>> Add(CreateLocationRequest createLocationRequest)
        {
            return await _locationService.CreateLocation(createLocationRequest);
        }   
        
        [HttpGet("{hash}")]
        public async Task<BaseRespons<LocationResponse>> GetLocation(Guid hash)
        {
            return await _locationService.GetLocationByHash(hash);
        }

        [HttpPut]
        public async Task<BaseRespons<LocationResponse>> UpdateLocation(UpdateLocationRequest updateLocationRequest)
        {
            return await _locationService.UpdateLocation(updateLocationRequest);
        }

        [HttpDelete("{hash}")]
        public async Task<BaseRespons> DeleteLocation(Guid hash)
        {
            await _locationService.DeleteLocation(hash);
            return BaseRespons.Success();
        }
    }
}
