using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Request.User;
using Models.Response._Core;
using Models.Response.User;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.HR, SystemRoles.CompanyOwner)]
        public async Task<BaseRespons<UserResponse>> Create(CreateUserRequest createRequest)
        {
            var response = await _userService.CreateUser(createRequest);
            await _userService.AddRoleToUser(response.Hash, createRequest.RoleIds);
            return response;
        }
    }
}
