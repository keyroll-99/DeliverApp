using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Request.User;
using Models.Response._Core;
using Models.Response.User;
using Services.Interface;

namespace Deliver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private const string RefrehTokenCookieName = "refreshToken";

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<BaseResponse<AuthResponse>> Login(LoginRequest loginRequest)
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
        public async Task<BaseResponse<AuthResponse>> RefreshToken()
        {
            var token = Request.Cookies[RefrehTokenCookieName];
            var ipAddress = getIpAddress() ?? "unknown";
            var response = await _userService.RefreshToken(token, ipAddress);
            if (response.IsSuccess)
            {
                setTokenCookie(response.Data!.RefreshToken);
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
            Response.Cookies.Append(RefrehTokenCookieName, token, cookieOption);
        }

        private string? getIpAddress() =>
            HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
