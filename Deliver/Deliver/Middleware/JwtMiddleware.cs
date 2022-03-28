using Models;
using Services.Interface;

namespace Deliver.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSetting;

    public JwtMiddleware(RequestDelegate requestDelegate, AppSettings appSetting)
    {
        _next = requestDelegate;
        _appSetting = appSetting;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if(userId is not null)
        {
            context.Items["User"] = await userService.GetById(userId.Value);
        }
        await _next(context);
    }
}
