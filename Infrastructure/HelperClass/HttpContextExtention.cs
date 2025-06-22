using System.Security.Claims;
using arkbo_inventory.Infrastructure.Data;

namespace arkbo_inventory.Infrastructure.Services;

public static class HttpContextExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null || !int.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }
        return userId;
    }

    public static string GetRole(this HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.Role);
        return claim?.Value ?? throw new UnauthorizedAccessException("Role claim is missing.");
    }
}
