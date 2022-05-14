using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Mapper;
using Models.Request.Location;
using Models.Request.Utils.Role;
using Models.Response.Location;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;

namespace Services.Impl;

public class LocationService : ILocationService
{
    private readonly ILocationReposiotry _locationReposiotry;
    private readonly LoggedUser _loggedUser;
    private readonly ICompanyUtils _companyUtils;
    private readonly IRoleUtils _roleUtils;

    public LocationService(
        ILocationReposiotry locationReposiotry,
        IOptions<LoggedUser> loggedUser,
        ICompanyUtils companyUtils,
        IRoleUtils roleUtils
        )
    {
        _locationReposiotry = locationReposiotry;
        _loggedUser = loggedUser.Value;
        _companyUtils = companyUtils;
        _roleUtils = roleUtils;
    }

    public async Task<LocationResponse> CreateLocation(CreateLocationRequest createLocationRequest)
    {
        if (!createLocationRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToLocationAction(PermissionActionEnum.Create))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        var userCompany = await _companyUtils.GetUserCompany(_loggedUser.Id);

        var location = createLocationRequest.CreateLocation(userCompany);
        await _locationReposiotry.AddAsync(location);

        return location.AsLocationResponse();
    }

    public async Task DeleteLocation(Guid hash)
    {
        var location = await _locationReposiotry.GetByHashAsync(hash);
        if (location is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }
        if (!await HasPermissionToLocationAction(PermissionActionEnum.Delete, location))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        await _locationReposiotry.DeleteAsync(location);
    }

    public async Task<LocationResponse> GetLocationByHash(Guid hash)
    {
        var location = await _locationReposiotry
            .GetAll()
            .FirstOrDefaultAsync(x => x.Hash == hash);

        if (location is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToLocationAction(PermissionActionEnum.Get, location))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        return location.AsLocationResponse();
    }

    public async Task<List<LocationResponse>> GetLocations()
    {
        if (!await HasPermissionToLocationAction(PermissionActionEnum.Get))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        var locations = await _locationReposiotry
            .GetAll()
            .Include(x => x.Company)
            .ThenInclude(x => x.Users)
            .Where(x => x.Company.Users.Any(u => u.Id == _loggedUser.Id))
            .ToListAsync();

        return locations.Select(x => x.AsLocationResponse()).ToList();
    }

    public async Task<LocationResponse> UpdateLocation(UpdateLocationRequest updateLocationRequest)
    {
        var location = await _locationReposiotry.GetByHashAsync(updateLocationRequest.Hash);
        if (location is null || !updateLocationRequest.IsValid)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        if (!await HasPermissionToLocationAction(PermissionActionEnum.Update, location))
        {
            throw new AppException(ErrorMessage.InvalidRole);
        }

        location = location.UpdateLocation(updateLocationRequest);
        await _locationReposiotry.UpdateAsync(location);

        return location.AsLocationResponse();
    }

    private async Task<bool> HasPermissionToLocationAction(PermissionActionEnum action, Location? location = null)
    {
        var hasPermission = await _roleUtils.HasPermission(new HasPermissionRequest
        {
            Action = action,
            PermissionTo = PermissionToEnum.Location,
            Roles = _loggedUser.Roles
        });

        var isSameCompany = true;
        if (location is not null)
        {
            var userCompany = await _companyUtils.GetUserCompany(location.CompanyId);
            isSameCompany = location.CompanyId == userCompany.Id;
        }

        return hasPermission && isSameCompany;
    }
}
