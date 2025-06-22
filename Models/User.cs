using arkbo_inventory.Models;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public int RoleId { get; set; }
    public Role? Role { get; set; }
    
    public int? createdById { get; set; }
    public User? CreatedBy { get; set; }
    
    public ICollection<UserDevices> UserDevices { get; set; } = new List<UserDevices>();
    public ICollection<DeviceAssignment> AssignmentsByUsers { get; set; } = new List<DeviceAssignment>();
    public ICollection<DeviceAssignment> AssignedToUsers { get; set; } = new List<DeviceAssignment>();

}