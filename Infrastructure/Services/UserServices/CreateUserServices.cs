using System.Security.Claims;
using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Infrastructure.Services.UserServices.UserDTOs;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Infrastructure.Services.UserServices;

public class CreateUserServices : IUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public CreateUserServices(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<IResult> CreateUser(CreateUserRequest request)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        var userCheck = await _context.arkbo_users
            .Where(u => u.Email == request.Email)
            .AnyAsync();

        if (userCheck)
            return Results.BadRequest("User with same email already exists");

        var currentUserId = httpContext?.GetUserId();
        
        var currentUserRole = await _context.arkbo_users
            .Where(u => u.UserId == currentUserId)
            .Select(u => u.RoleId)
            .FirstOrDefaultAsync();
        
        if (currentUserRole == 0)
            return Results.BadRequest("Creator's Role could not be determined");
        
        if(currentUserRole>=request.RoleId)
            return Results.BadRequest("Creator's Role cannot be smaller or equal to User's Role");
        
        var user = new User
        {
            Email = request.Email,
            Password = request.Password,
            Phone = request.Phone,
            RoleId = request.RoleId,
            createdById = currentUserId
        };
        await _context.arkbo_users.AddAsync(user);
        await _context.SaveChangesAsync();
        return Results.Ok("User has been created created");
    }

    public async Task<List<UserResponseDTO>> GetAllUsers()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("User not authorized");
        }

        var currentUserId = httpContext.GetUserId();
        var currentUserRole = httpContext.GetRole();
        
        if(currentUserId <= 0 || string.IsNullOrEmpty(currentUserRole))
            throw new UnauthorizedAccessException("Invalid user ID or role.");

        if (currentUserRole == "Admin")
        {
            return await _context.arkbo_users
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    PhoneNumber = u.Phone
                }).ToListAsync();
        }
        else
        {
            return await _context.arkbo_users
                .Where(u => u.createdById == currentUserId)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    PhoneNumber = u.Phone
                }).ToListAsync();
        }
    }

    public async Task<UserResponseDTO> GetUserById(int userId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("User not authorized");
        }
        var currentUserId = httpContext.GetUserId();
        var currentUserRole = httpContext.GetRole();

        var user = await _context.arkbo_users.FindAsync(userId);
        if (user == null)
           throw new UnauthorizedAccessException("User not authorized");
        
        if(currentUserId <= 0 || string.IsNullOrEmpty(currentUserRole))
            throw new UnauthorizedAccessException("Invalid user ID or role.");

        if (currentUserRole == "Admin")
        {
             var result=await _context.arkbo_users
                .Where(u => u.UserId == userId)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    PhoneNumber = u.Phone,
                    RoleId = u.RoleId
                })
                .SingleOrDefaultAsync();
             if (result == null)
                 throw new KeyNotFoundException("User not found");

             return result;
        }
        else
        {
            var result = await _context.arkbo_users
                .Where(u => u.UserId == userId && u.createdById == currentUserId)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    PhoneNumber = u.Phone,
                    RoleId = u.RoleId
                }).SingleOrDefaultAsync();
            if (result == null)
                throw new KeyNotFoundException("User not found");

            return result;
        }
    }
    public async Task <IResult>DeleteUser(int userId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("User not authorized");
        }
        var currentUserId = httpContext.GetUserId();
        var currentUserRole = httpContext.GetRole();

        var user = await _context.arkbo_users.FindAsync(userId);
        if (user == null)
            return Results.NotFound("User not Found");
        
        if(currentUserId <= 0 || string.IsNullOrEmpty(currentUserRole))
            return Results.BadRequest("Error while parsing user ID or role.");

        if (currentUserRole == "Admin")
        {
            _context.arkbo_users.Remove(user);
            return Results.Ok("User has been deleted");
        }
        
        var validateUserId = await _context.arkbo_users
            .Where(u=>u.createdById==currentUserId && u.UserId == userId)
            .FirstOrDefaultAsync();
        if(validateUserId == null)
            return Results.Unauthorized();
        _context.arkbo_users.Remove(validateUserId);
        await _context.SaveChangesAsync();
        return Results.Ok("User has been deleted");
    }
    
    
}