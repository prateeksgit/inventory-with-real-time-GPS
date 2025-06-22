using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using arkbo_inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace arkbo_inventory.Infrastructure.Auth;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;

    public JwtService(IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleDescription)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User?> ValidateUser(string email, string password)
    {
        return await _dbContext.arkbo_users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }
} 