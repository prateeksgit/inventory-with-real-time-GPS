using arkbo_inventory.Features.Devices;
using arkbo_inventory.Infrastructure.Services.UserServices.UserDTOs;
using Microsoft.AspNetCore.Mvc;

namespace arkbo_inventory.Infrastructure.Services.UserServices;

public interface IUserServices
{
    public Task<IResult>CreateUser(CreateUserRequest request);
    public Task<List<UserResponseDTO>>GetAllUsers();
    
    public Task<UserResponseDTO> GetUserById(int id);
    
    public Task<IResult>DeleteUser(int id);
}