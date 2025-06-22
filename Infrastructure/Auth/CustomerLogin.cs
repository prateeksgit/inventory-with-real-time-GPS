using arkbo_inventory.Infrastructure.Data;
using arkbo_inventory.Infrastructure.Services;

namespace arkbo_inventory.Infrastructure.Auth;

public static class CustomerLogin
{
    public static IEndpointRouteBuilder MapCustomerLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/CustomerLogin", async (
                LoginRequest loginRequest,
                JwtService jwtService,
                TraccarService traccarService) =>
            {
                var user=await jwtService.ValidateUser(loginRequest.Email,loginRequest.Password);
                if (user == null)
                {
                    return Results.BadRequest("Invalid credentials");
                }

                if (user.Role.RoleDescription == "Admin")
                {
                    return Results.BadRequest("Login through admin page");
                }

                var token = jwtService.GenerateToken(user);

                var traccarToken = await traccarService.LoginAndGetTokenAsync();
                if (traccarToken == null)
                {
                    return Results.BadRequest("Token couldn't be generated.");
                }

                return Results.Ok(new LoginResponse(
                    Token:token,
                    TraccarToken:traccarToken
                ));
            }).WithTags("Customer");
        return endpoints;
    }
}