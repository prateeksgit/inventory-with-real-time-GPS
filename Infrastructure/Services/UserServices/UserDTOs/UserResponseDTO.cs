namespace arkbo_inventory.Infrastructure.Services.UserServices.UserDTOs;

public class UserResponseDTO
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public int RoleId { get; set; }
}