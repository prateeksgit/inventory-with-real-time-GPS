using arkbo_inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace arkbo_inventory.Infrastructure.Data;

public class DeviceAssignmentConfiguration : IEntityTypeConfiguration<DeviceAssignment>
{
    
    public void Configure(EntityTypeBuilder<DeviceAssignment> entity)
    {
        entity.ToTable("arkbo_deviceassignments"); 
        
        entity.HasKey(da => da.AssignmentId);
        
        entity.HasOne(da=>da.Device)
            .WithMany(d=>d.DeviceAssignments)
            .HasForeignKey(da => da.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(da=>da.AssignedTo)
            .WithMany(d=>d.AssignedToUsers)
            .HasForeignKey(da => da.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(da=>da.AssignedBy)
            .WithMany(d=>d.AssignmentsByUsers)
            .HasForeignKey(da => da.AssignedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        entity.Property(da => da.AssignmentDate).IsRequired();
        entity.Property(da => da.IsActive).IsRequired();

    }
    
}