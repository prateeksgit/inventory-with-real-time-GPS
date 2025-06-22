namespace arkbo_inventory.Models;

public class DeviceAssignment
{
    public int AssignmentId { get; set; }
    
    public int DeviceId { get; set; }
    public Device Device { get; set; } = null!;
    
    public int AssignedById { get; set; }
    public User AssignedBy { get; set; } = null!;
    
    public int AssignedToId { get; set; }
    public User AssignedTo { get; set; } = null!;
    
    public DateTime AssignmentDate { get; set; }
    public bool IsActive { get; set; }
}