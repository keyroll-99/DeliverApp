using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Db.ConstValues;
using Models.Response.Role;
using Repository.Repository.Interface;
using Services.Interface;
using Services.Interface.Utils;

namespace Services.Impl;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly LoggedUser _loggedUser;

    public RoleService(
        IRoleRepository roleRepository,
        IOptions<LoggedUser> loggedUser
        )
    {
        _roleRepository = roleRepository;
        _loggedUser = loggedUser.Value;
    }

    public async Task<List<RoleResponse>> GetAllAvailable()
    {
        var roles = await _roleRepository
            .GetAll()
            .Select(x => new RoleResponse
                {
                    Name = x.Name,
                    Id = x.Id
                })
            .ToListAsync();

        if (!_loggedUser.Roles.Contains(SystemRoles.Admin))
        {
            roles.RemoveAll(x => x.Name == SystemRoles.Admin);
        }
        return roles;
    }
}
