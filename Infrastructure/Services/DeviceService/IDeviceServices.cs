using System.Security.Claims;
using arkbo_inventory.Features.Devices;
using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Infrastructure.Services;
public interface IDeviceService
{
    Task<(bool Success, string ErrorMessage)> AssignDeviceAsync(AssignDeviceRequest request);
    Task<List<DevicesResponse>> GetAllDevices();
    Task<IResult> CreateDevice(CreateDeviceDto request);
    
    Task<IResult> DeleteDevice(int deviceId);
    
    Task<IResult>UpdateDevice(int id,UpdateDeviceRequest request);


}

