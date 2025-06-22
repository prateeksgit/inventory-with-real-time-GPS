using arkbo_inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace arkbo_inventory.Infrastructure.Data;

public class UserDevicesNormalizationConfiguration : IEntityTypeConfiguration<UserDevices>
{
    public void Configure(EntityTypeBuilder<UserDevices> entity)
    {
        entity.ToTable("arkbo_userdevices");
        entity.HasKey(ud => new { ud.UserId, ud.DeviceId });
            
            entity.HasOne(ud => ud.User)
            .WithMany(u => u.UserDevices)
            .HasForeignKey(ud => ud.UserId);

        entity.HasOne(ud => ud.Device)
            .WithMany(d => d.UserDevices)
            .HasForeignKey(ud => ud.DeviceId);
    }
}