namespace arkbo_inventory.Models;

public class UserDevices
{
    public int UserId { get; set; }
    public int DeviceId { get; set; }
    
    public User User { get; set; } = null!;
    public Device Device { get; set; } = null!;
}