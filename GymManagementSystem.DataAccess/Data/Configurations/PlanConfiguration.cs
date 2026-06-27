using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(200)
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(p => p.DurationDays)
            .HasColumnOrder(4)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(10,2)
            .HasColumnOrder(5)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .HasDefaultValue("false")
            .HasColumnOrder(6)
            .IsRequired();

        builder.HasMany(p => p.Memberships)
            .WithOne(ms => ms.Plan)
            .HasForeignKey(ms => ms.PlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Plans_DurationDays", "(DurationDays BETWEEN 1 AND 365)");
        });
    }
}
