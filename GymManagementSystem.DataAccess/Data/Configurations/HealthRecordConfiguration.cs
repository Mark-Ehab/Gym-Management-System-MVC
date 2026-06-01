using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.Property(hr => hr.Height)
            .HasPrecision(5, 2)
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(hr => hr.Weight)
            .HasPrecision(5,2)
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(hr => hr.BloodType)
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasColumnOrder(4);

        builder.Property(hr => hr.Note)
            .HasMaxLength(1000)
            .HasColumnOrder(5);

        builder.Property(hr => hr.LastUpdate)
            .HasDefaultValueSql("SYSDATETIME()")
            .HasColumnOrder(6)
            .IsRequired();

        builder.HasIndex(hr => hr.MemberId)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne(hr => hr.Member)
            .WithOne(m => m.HealthRecord)
            .HasForeignKey<HealthRecord>(hr => hr.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
