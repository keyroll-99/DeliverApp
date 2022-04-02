using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Db;

namespace Deliver.CustomAttribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string? _requireRole;

    public AuthorizeAttribute()
    {
    }

    public AuthorizeAttribute(string requireRole)
    {
        _requireRole = requireRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        var user = context.HttpContext.Items["User"] as User;
        var roles = context.HttpContext.Items["Roles"] as List<string>;

        var haveValidRole = _requireRole is not null
            ? roles?.Contains(_requireRole)
            : true;

        if (user is null || !haveValidRole.GetValueOrDefault(false))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
