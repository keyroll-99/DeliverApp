using Models.Response.Role;

namespace Services.Interface;

public interface IRoleService
{
    public Task<List<RoleResponse>> GetAllAvailable();
}
