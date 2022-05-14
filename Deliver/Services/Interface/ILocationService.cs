using Models.Request.Location;
using Models.Response.Location;

namespace Services.Interface;

public interface ILocationService
{
    public Task<List<LocationResponse>> GetLocations();
    public Task<LocationResponse> CreateLocation(CreateLocationRequest createLocationRequest);
    public Task<LocationResponse> GetLocationByHash(Guid hash);
    public Task<LocationResponse> UpdateLocation(UpdateLocationRequest updateLocationRequest);
    public Task DeleteLocation(Guid hash);
}
