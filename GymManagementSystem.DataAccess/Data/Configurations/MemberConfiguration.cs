using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class MemberConfiguration : GymUserConfiguration<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.Property(m => m.JoinDate)
            .HasDefaultValueSql("SYSDATETIME()")
            .HasColumnOrder(10)
            .IsRequired();

        builder.Property(m => m.Photo)
            .HasColumnOrder(11);

        builder.HasOne(m => m.HealthRecord)
            .WithOne(hr => hr.Member)
            .HasForeignKey<HealthRecord>(hr => hr.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Bookings)
            .WithOne(b => b.Member)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Memberships)
            .WithOne(ms => ms.Member)
            .HasForeignKey(ms => ms.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
