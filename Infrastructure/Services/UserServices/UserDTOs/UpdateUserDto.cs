namespace arkbo_inventory.Infrastructure.Services.UserServices.UserDTOs;

public class UpdateUserDto
{
    public string Email { get; set; }=string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    public int RoleId { get; set; }
    
}