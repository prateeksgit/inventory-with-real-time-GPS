using System.Security.Claims;
using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Infrastructure.Services;
using arkbo_inventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Features.Devices;
public record AssignDeviceRequest(int DeviceIdToAssign, int ToUserId);

public static class DeviceEndpoints
{
    public static IEndpointRouteBuilder MapDeviceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/devices", [Authorize] async (
            [FromServices] IDeviceService deviceService) =>
        {
            var devices = await deviceService.GetAllDevices();
            return Results.Ok(devices);
        }).WithTags("Admin and User");

        endpoints.MapPost("/devices", [Authorize(Roles = "Admin")] async (
                [FromServices] IDeviceService deviceService,
                [FromBody] CreateDeviceDto createDeviceDto
            ) =>
            await deviceService.CreateDevice(createDeviceDto)).WithTags("Admin");

        endpoints.MapPost("/assign-device", async (
            [FromServices] IDeviceService deviceService,
            AssignDeviceRequest request
        ) =>
        {
            var (success, error) = await deviceService.AssignDeviceAsync(request);
            if (!success)
            {
                return Results.BadRequest(new { message = error });
            }

            return Results.Ok(new { message = "Device assigned successfully" });
        }).WithTags("Admin and User");

        endpoints.MapDelete("/devices/{deviceId:int}", [Authorize(Roles = "Admin")] async (
            [FromServices] IDeviceService deviceService,
            int deviceId) =>
                 await deviceService.DeleteDevice(deviceId)).WithTags("Admin");

        endpoints.MapPut("/devices/{deviceId:int}",[Authorize(Roles = "Admin")] async (
            [FromServices] IDeviceService deviceService,
            UpdateDeviceRequest request,
            int deviceId) =>
             await deviceService.UpdateDevice(deviceId, request)).WithTags("Admin");

        endpoints.MapGet("/devicesfromTraccar", [Authorize(Roles = "Admin")] async (
            [FromServices] TraccarService traccarService) =>
        {
            var devices = await traccarService.GetDevicesForUserAsync();
            return Results.Ok(devices);
        }).WithTags("Admin");

        endpoints.MapGet("devices/{deviceId:int}/deployDeviceToTraccar", [Authorize] async (
                [FromServices] TraccarService traccarService,
                int deviceId) =>
            await traccarService.DeployDeviceToTraccar(deviceId)).WithTags("Admin and User");

        return endpoints;
    }
}