namespace arkbo_inventory.Infrastructure.Services.UserServices.UserDTOs;

public class CreateUserRequest
{
    public string Email { get; set; }=string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; }
    public int RoleId { get; set; }
}