using arkbo_inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace arkbo_inventory.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<DeviceAssignment> arkbo_deviceassignments { get; set; }
    public DbSet<UserDevices> arkbo_userdevices { get; set; }
    public DbSet<User> arkbo_users => Set<User>();
    public DbSet<Device> arkbo_devices => Set<Device>();
    public DbSet<Role> arkbo_roles => Set<Role>();
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DeviceAssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new UserCreatedByConfiguration());
        modelBuilder.ApplyConfiguration(new UserDevicesNormalizationConfiguration());
    }
}