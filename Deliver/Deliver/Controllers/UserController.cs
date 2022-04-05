using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Db.ConstValues;
using Models.Exceptions;
using Models.Request.User;
using Models.Response._Core;
using Models.Response.User;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private const string _refrehTokenCookieName = "refreshToken";

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<BaseRespons<AuthResponse>> Login(LoginRequest loginRequest)
        {
            var ipAddress = getIpAddress() ?? "unknown";
            var response = await _userService.Login(loginRequest, ipAddress);
            if (response.IsSuccess)
            {
                setTokenCookie(response.Data!.RefreshToken);
            }
            return response;
        }

        [HttpPost("Refresh")]
        [AllowAnonymous]
        public async Task<BaseRespons<AuthResponse>> RefreshToken()
        {
            var token = Request.Cookies[_refrehTokenCookieName];
            var ipAddress = getIpAddress() ?? "unknown";
            var response = await _userService.RefreshToken(token, ipAddress);
            if (response.IsSuccess)
            {
                setTokenCookie(response.Data!.RefreshToken);
            }
            return response;
        }

        [HttpPost("Create")]
        [Authorize(SystemRoles.Admin, SystemRoles.CompanyAdmin, SystemRoles.HR, SystemRoles.CompanyOwner)]
        public async Task<BaseRespons<UserReponse>> Create(CreateUserRequest createRequest)
        {
            var response = await _userService.CreateUser(createRequest);
            if (response.IsSuccess)
            {
                await _userService.AddRoleToUser(response.Data.Hash, createRequest.RoleIds);
            }
            return response;
        }

        private void setTokenCookie(string token)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            Response.Cookies.Append(_refrehTokenCookieName, token, cookieOption);
        }

        private string? getIpAddress() =>
            HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
