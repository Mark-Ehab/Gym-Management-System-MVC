using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class TrainerConfiguration : GymUserConfiguration<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Speciality)
            .HasConversion<string>()
            .HasMaxLength(16)
            .HasColumnOrder(10)
            .IsRequired();

        builder.Property(t => t.HireDate)
            .HasDefaultValueSql("SYSDATETIME()")
            .HasColumnOrder(11)
            .IsRequired();

        builder.HasMany(t => t.Sessions)
            .WithOne(s => s.Trainer)
            .HasForeignKey(s => s.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
