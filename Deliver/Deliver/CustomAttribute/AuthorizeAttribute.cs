using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Db;

namespace Deliver.CustomAttribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly List<string> _requireRole = new();

    public AuthorizeAttribute()
    {
    }

    public AuthorizeAttribute(params string[] requireRole)
    {
        _requireRole = requireRole.ToList();
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

        var haveValidRole = _requireRole is not null && _requireRole.Any()
            ? roles?.Any(x => _requireRole.Contains(x))
            : true;

        if (user is null || !haveValidRole.GetValueOrDefault(false))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
