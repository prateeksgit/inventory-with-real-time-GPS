public class Role
{
    public int RoleId { get; set; }
    public string RoleDescription { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
    
    public enum UserRole
    {
        Admin = 1,
        Distributor = 2,    
        Retailer = 3,
        Customer = 4
    }
} 