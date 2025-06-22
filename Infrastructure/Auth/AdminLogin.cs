using arkbo_inventory.Infrastructure.Services;

namespace arkbo_inventory.Infrastructure.Auth;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token, string TraccarToken);

public static class AdminLogin
{
    public static IEndpointRouteBuilder MapLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/Adminlogin", async (
            LoginRequest request,
            JwtService jwtService,
            TraccarService traccarService) =>
        {
            var user = await jwtService.ValidateUser(request.Email, request.Password);
            if (user == null)
                return Results.Unauthorized();

            if (user.Role.RoleDescription != "Admin")
                return Results.Unauthorized();
            
            var token = jwtService.GenerateToken(user);
            var traccarToken = await traccarService.LoginAndGetTokenAsync();

            if (traccarToken == null)
            {
                Console.WriteLine("TraccarToken is null");
            }

            return Results.Ok(new LoginResponse(
                Token: token,
                TraccarToken: traccarToken
            ));
        }).WithTags("Admin");

        return endpoints;
    }
}