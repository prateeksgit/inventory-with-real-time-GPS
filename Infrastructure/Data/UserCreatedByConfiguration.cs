using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace arkbo_inventory.Infrastructure.Data;

public class UserCreatedByConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("arkbo_users");
        entity.HasOne(u=>u.CreatedBy)
            .WithMany()
            .HasForeignKey(u=>u.createdById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}