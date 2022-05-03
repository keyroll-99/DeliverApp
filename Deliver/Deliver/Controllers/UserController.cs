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
    [Authorize]
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

        [HttpPut("ChangePassword")]
        public async Task<BaseRespons> UpdatePassword(ChangePasswordRequest updatePasswordRequest)
        {
            await _userService.UpdatePassword(updatePasswordRequest);
            return BaseRespons.Success();
        }

        [HttpPut("Update")]
        public async Task<BaseRespons<UserResponse>> Update(UpdateUserRequest updateUserRequest)
        {
            return await _userService.UpdateUser(updateUserRequest);
        }

        [HttpGet("{userHash}")]
        public async Task<BaseRespons<UserResponse>> GetUser(Guid userHash)
        {
            return await _userService.GetUser(userHash);
        }
    }
}
