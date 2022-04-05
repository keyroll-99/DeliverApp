using Microsoft.Extensions.Options;
using Models;
using Services.Interface.Utils;

namespace Deliver.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<LoggedUser> _loggedUser;

    public JwtMiddleware(RequestDelegate requestDelegate, IOptions<LoggedUser> options)
    {
        _next = requestDelegate;
        _loggedUser = options;
    }

    public async Task Invoke(HttpContext context, IUserUtils userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);

        if (userId is not null)
        {
            var user = await userService.GetById(userId.Value);
            if (user is not null)
            {
                _loggedUser.Value.Id = user.Id;
                _loggedUser.Value.Roles = user.UserRole.Select(x => x.Role.Name).ToList();
                context.Items["User"] = user;
                context.Items["Roles"] = _loggedUser.Value.Roles;
            }
        }

       await _next(context);
    }
}
