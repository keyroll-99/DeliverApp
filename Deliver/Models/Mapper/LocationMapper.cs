using Models.Db;
using Models.Request.Location;
using Models.Response.Location;

namespace Models.Mapper;

public static class LocationMapper
{
    public static LocationResponse AsLocationResponse(this Location location)
    {
        return new LocationResponse
        {
            Hash = location.Hash,
            City = location.City,
            Country = location.Country,
            Email = location.Email,
            No = location.No,
            PhoneNumber = location.PhoneNumber,
            PostalCode = location.PostalCode,
            Region = location.Region,
            Street = location.Street
        };
    }

    public static Location CreateLocation(this CreateLocationRequest locationRequest, Company company)
    {
        return new Location
        {
            City = locationRequest.City,
            Company = company,
            Country = locationRequest.Country,
            CompanyId = company.Id,
            Email = locationRequest.Email,
            Hash = Guid.NewGuid(),
            No = locationRequest.No,
            PhoneNumber = locationRequest.PhoneNumber,
            Region = locationRequest.Region,
            Street = locationRequest.Street,
            PostalCode = locationRequest.PostalCode,
        };
    }

    public static Location UpdateLocation(this Location location, UpdateLocationRequest update)
    {
        location.City = update.City;
        location.Country = update.Country;
        location.Email = update.Email;
        location.No = update.No;
        location.PhoneNumber = update.PhoneNumber;
        location.PostalCode = update.PostalCode;
        location.Region = update.Region;
        location.Street = update.Street;
        return location;
    }
}
