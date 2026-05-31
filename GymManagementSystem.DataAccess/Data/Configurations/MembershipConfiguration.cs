using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.Property(ms => ms.StartDate)
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.HasOne(ms => ms.Member)
            .WithMany(m => m.Memberships)
            .HasForeignKey(ms => ms.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ms => ms.Plan)
            .WithMany(p => p.Memberships)
            .HasForeignKey(ms => ms.PlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
