using System.Security.Claims;
using arkbo_inventory.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Features.Devices;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomer(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/customer/devices",[Authorize]async (
            HttpContext httpContext,
            ApplicationDbContext db) =>
        {
            var currentUserIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out var currentUserId))
            {
                throw new UnauthorizedAccessException("User ID claim missing or invalid.");
            }

            var devices = await db.arkbo_userdevices
                .Where(d => d.UserId==currentUserId)
                .Select(d => new
                {
                    d.Device.DeviceId,
                    d.Device.Name,
                    d.Device.UniqueId
                }).ToListAsync();
            return Results.Ok(devices);
        }).WithTags("Customer");
        return endpoints;
    }
}