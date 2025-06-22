using arkbo_inventory.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Features.Users;

public record RoleCreateRequest(string RoleDescription);

public static class RoleEndpoints
{
    public static IEndpointRouteBuilder MapRoles(this IEndpointRouteBuilder endpoints)
    {
        // Get all roles
        endpoints.MapGet("/roles", async (ApplicationDbContext db) =>
        {
            var roles = await db.arkbo_roles
                .Select(r => new
                {
                    r.RoleId,
                    r.RoleDescription,
                    UserCount = r.Users.Count
                })
                .ToListAsync();
            return Results.Ok(roles);
        }).WithTags("Roles");
        return endpoints;
    }
} 