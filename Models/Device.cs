using arkbo_inventory.Models;

public class Device
{
    public int DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string UniqueId { get; set; } = string.Empty;
    
    public ICollection<UserDevices> UserDevices { get; set; } = new List<UserDevices>();
    public ICollection<DeviceAssignment> DeviceAssignments { get; set; } = new List<DeviceAssignment>();
}