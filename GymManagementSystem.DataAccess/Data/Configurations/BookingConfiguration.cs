using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasIndex(b => new
        {
            b.MemberId,
            b.SessionId
        }).IsUnique();

        builder.Property(b => b.BookingDate)
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();

        builder.HasOne(b => b.Session)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Member)
            .WithMany(m => m.Bookings)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
