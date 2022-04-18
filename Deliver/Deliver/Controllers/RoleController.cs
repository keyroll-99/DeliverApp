using Microsoft.AspNetCore.Mvc;
using Models.Response._Core;
using Models.Response.Role;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<BaseRespons<List<RoleResponse>>> GetRoles()
        {
            return await _roleService.GetAllAvailable();
        }
    }
}
