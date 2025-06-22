using System.Security.Claims;
using arkbo_inventory.Features.Devices;
using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Infrastructure.Services;

public class CreateDeviceService : IDeviceService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateDeviceService(ApplicationDbContext context,IHttpContextAccessor  httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Success, string ErrorMessage)> AssignDeviceAsync(AssignDeviceRequest request)
    {
        var alreadyAssigned =  await _context.arkbo_userdevices
            .Where(u => u.UserId == request.ToUserId && u.DeviceId == request.DeviceIdToAssign).AnyAsync();
        if (alreadyAssigned)
            return (false,"User already assigned to this device");
        
        var device = await _context.arkbo_devices.FindAsync(request.DeviceIdToAssign);
        if (device == null)
            return(false,"Device not found");
        
        var user= await _context.arkbo_users.FindAsync(request.ToUserId);
        if (user == null)
            return(false,"User not found");
        
        var currentUserIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out var deviceAssigner))
        {
            var error = new UnauthorizedAccessException("User ID claim missing or invalid.");
            return (false, "User ID claim missing or invalid.");
        }
        var ToUserRole = user.RoleId;
        var fromUserRole = await _context.arkbo_users
            .Where(u => u.UserId == deviceAssigner)
            .Select(u => u.RoleId).FirstOrDefaultAsync() ;

        var readyForAssign = await _context.arkbo_userdevices
            .Where(ud => ud.UserId == deviceAssigner && ud.DeviceId == request.DeviceIdToAssign)
            .AnyAsync();
        
        if (ToUserRole <= fromUserRole)
            return (false, "Cannot assign device to user with same or higher role");
        
        if (!readyForAssign && deviceAssigner!=1) //don't use 1 here, need to extract admin description!!!!!!!!!!!!!!!!!
            return (false, "Admin hasn't assigned this device yet.Contact Admin");
        
        var assignerAlreadyAssigned= await _context.arkbo_userdevices
            .Where(u => u.UserId == deviceAssigner && u.DeviceId == request.DeviceIdToAssign)
            .AnyAsync();

        if (!assignerAlreadyAssigned)
        {
            var assignerEntry = new UserDevices
            {
                DeviceId = request.DeviceIdToAssign,
                UserId = deviceAssigner
            };
            _context.arkbo_userdevices.Add(assignerEntry);
        }
        var newAssignment = new UserDevices
        {
            DeviceId = request.DeviceIdToAssign,
            UserId = request.ToUserId
        };
        _context.arkbo_userdevices.Add(newAssignment);

        var newAssignmentData = new DeviceAssignment
        {
            DeviceId = request.DeviceIdToAssign,
            AssignedById = deviceAssigner,
            AssignedToId = request.ToUserId,
            AssignmentDate = DateTime.UtcNow,
            IsActive = true
        };
        _context.arkbo_deviceassignments.Add(newAssignmentData);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<List<DevicesResponse>> GetAllDevices()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            throw new UnauthorizedAccessException("HttpContext not available.");

        var userId = httpContext.GetUserId(); 
        var role = httpContext.GetRole(); 

        if (userId <= 0 || string.IsNullOrEmpty(role))
            throw new UnauthorizedAccessException("Invalid user ID or role.");

        if (role == "Admin")
        {
            return await _context.arkbo_devices
                .Select(d => new DevicesResponse
                {
                    DeviceId = d.DeviceId,
                    DeviceName = d.Name,
                    UniqueId = d.UniqueId
                }).ToListAsync();
        }
        else
        {
            return await _context.arkbo_devices
                .Where(ud => ud.UserDevices.Any(u => u.UserId == userId))
                .Select(d => new DevicesResponse
                {
                    DeviceId = d.DeviceId,
                    DeviceName = d.Name,
                    UniqueId = d.UniqueId
                }).ToListAsync();
        }
    }

    public async Task<IResult> CreateDevice(CreateDeviceDto request)
    {
        var deviceCheck = await _context.arkbo_devices
            .Where(d => request.UniqueId == d.UniqueId)
            .AnyAsync();
        if (deviceCheck)
        {
            return Results.BadRequest("Device already assigned to this device");
        }

        var device = new Device
        {
           Name = request.DeviceName,
           UniqueId = request.UniqueId
        };
       await _context.arkbo_devices.AddAsync(device);
       await _context.SaveChangesAsync();
       return Results.Created($"/devices/{device.DeviceId}", device);
    }

    public async Task<IResult> DeleteDevice(int deviceId)
    {
        var device=await  _context.arkbo_devices.FindAsync(deviceId);
        if (device == null)
        {
            return Results.BadRequest("Device not found");
        }
        _context.arkbo_devices.Remove(device);
        await _context.SaveChangesAsync();
        return Results.Ok("Device deleted");
    }

    public async Task<IResult> UpdateDevice(int id,UpdateDeviceRequest request)
    {
        var device=await _context.arkbo_devices.FindAsync(id);
        if (device==null)
        {
            return Results.BadRequest("Device not found");
        }
        
        device.Name = request.DeviceName;
        device.UniqueId = request.UniqueId;
        await _context.SaveChangesAsync();
        return Results.Ok("Device updated");
    }
}