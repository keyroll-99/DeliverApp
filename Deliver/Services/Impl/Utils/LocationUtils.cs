using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db;
using Models.Exceptions;
using Repository.Repository.Interface;
using Services.Interface.Utils;

namespace Services.Impl.Utils;

public class LocationUtils : ILocationUtils
{
    private readonly ILocationReposiotry _locationReposiotry;
    private readonly LoggedUser _loggedUser;

    public LocationUtils(ILocationReposiotry locationReposiotry, IOptions<LoggedUser> loggedUser)
    {
        _locationReposiotry = locationReposiotry;
        _loggedUser = loggedUser.Value;
    }

    public async Task<Location> GetLoctaionByHash(Guid hash)
    {
        var location = await _locationReposiotry
            .GetAll()
            .Include(x => x.Company)
            .ThenInclude(x => x.Users)
            .FirstOrDefaultAsync(x =>
                x.Hash == hash
                && x.Company.Users.Any(y => y.Id == _loggedUser.Id)
            );

        if (location is null)
        {
            throw new AppException(ErrorMessage.InvalidData);
        }

        return location;
    }
}
