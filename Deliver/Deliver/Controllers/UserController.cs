using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Request.Account;
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
        
        [HttpGet("List")]
        [Authorize(SystemRoles.HR, SystemRoles.CompanyAdmin, SystemRoles.Admin, SystemRoles.CompanyOwner)]
        public async Task<List<UserResponse>> GetList()
        {
            return await _userService.GetUserList();
        }

        [HttpGet("Drivers")]
        [Authorize(SystemRoles.HR, SystemRoles.CompanyAdmin, SystemRoles.Admin, SystemRoles.CompanyOwner)]
        public async Task<List<UserResponse>> GetFilterList()
        {
            return await _userService.GetUsersWithRole(SystemRoles.Driver);
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.HR, SystemRoles.CompanyOwner)]
        public async Task<UserResponse> Create(CreateUserRequest createRequest)
        {
            var response = await _userService.CreateUser(createRequest);
            await _userService.AddRoleToUser(response.Hash, createRequest.RoleIds);
            return response;
        }

        [HttpPut("Fire/{userHash}")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.CompanyOwner, SystemRoles.HR)]
        public async Task<BaseRespons> FireUser(Guid userHash)
        {
            await _userService.FireUser(userHash);
            return BaseRespons.Success();
        }

        [HttpPut("Update")]
        public async Task<UserResponse> Update(UpdateUserRequest updateUserRequest)
        {
            return await _userService.UpdateUser(updateUserRequest);
        }

        [HttpGet("{userHash}")]
        public async Task<UserResponse> GetUser(Guid userHash)
        {
            return await _userService.GetUser(userHash);
        }
    }
}
