using System.Security.Claims;
using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Infrastructure.Services.UserServices;
using arkbo_inventory.Infrastructure.Services.UserServices.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Features.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Get all users
        endpoints.MapGet("/users", [Authorize] async
            (IUserServices userServices
            ) =>
            await userServices.GetAllUsers()).WithTags("Admin and User");
        
        // Create user
        endpoints.MapPost("/CreateUser", [Authorize] async (
                IUserServices userServices,
                CreateUserRequest request
            ) =>
            await userServices.CreateUser(request)).WithTags("Admin and User");

        // Get user by ID
        endpoints.MapGet("/users/{userId:int}", [Authorize] async (IUserServices userServices, int userId
            ) =>
            await userServices.GetUserById(userId)).WithTags("Admin and User");

        // Delete user
        endpoints.MapDelete("/users/{userId:int}", (
                IUserServices userService, int userId)
            => userService.DeleteUser(userId)).WithTags("Admin and User");
        
        return endpoints;
    }
}