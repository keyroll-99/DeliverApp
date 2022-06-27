using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Authentication;
using Models.Response._Core;
using Models.Response.Authentication;
using Services.Interface;

namespace Deliver.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticateService;
    private const string _refrehTokenCookieName = "refreshToken";

    public AuthenticationController(IAuthenticationService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<AuthResponse> Login(LoginRequest loginRequest)
    {
        var ipAddress = getIpAddress() ?? "unknown";
        var response = await _authenticateService.Login(loginRequest, ipAddress);
        setTokenCookie(response.RefreshToken);
        return response;
    }

    [HttpPost("Refresh")]
    [AllowAnonymous]
    public async Task<AuthResponse> RefreshToken()
    {
        var token = Request.Cookies[_refrehTokenCookieName];
        var ipAddress = getIpAddress() ?? "unknown";
        var response = await _authenticateService.RefreshToken(token, ipAddress);
        setTokenCookie(response.RefreshToken);
        return response;
    }

    [HttpPost("Logout")]
    [Authorize]
    public async Task<BaseRespons> Logout()
    {
        await _authenticateService.Logout(getIpAddress() ?? "unknow");
        return BaseRespons.Success();
    }

    [HttpGet("Permission")]
    [Authorize]
    public async Task<PermissionResponse> GetPermissions()
        => await _authenticateService.GetLoggedUserPermission();



    private void setTokenCookie(string token)
    {
        var cookieOption = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            Secure = true,
            SameSite = SameSiteMode.None
        };
        Response.Cookies.Append(_refrehTokenCookieName, token, cookieOption);
    }

    private string? getIpAddress() =>
        HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
}
