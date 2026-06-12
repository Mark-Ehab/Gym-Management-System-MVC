using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(s => s.Description)
            .HasMaxLength(400)
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(s => s.Capacity)
             .HasColumnOrder(3)
            .IsRequired();

        builder.Property(s => s.StartDate)
            .HasColumnOrder(4)
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnOrder(5)
            .IsRequired();

        builder.HasOne(s => s.Trainer)
            .WithMany(t => t.Sessions)
            .HasForeignKey(s => s.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Category)
            .WithMany(c => c.Sessions)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Bookings)
            .WithOne(b => b.Session)
            .HasForeignKey(b => b.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Sessions_Capacity", "(Capacity BETWEEN 1 AND 25)");
            t.HasCheckConstraint("CK_Sessions_StartDate_EndDate", "(EndDate > StartDate)");
        });
    }
}
