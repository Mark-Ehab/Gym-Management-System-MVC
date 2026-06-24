using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Data.Configurations;

public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(u => u.Name)
            .HasMaxLength(50)
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .HasColumnOrder(3)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.Property(u => u.Phone)
            .HasMaxLength(14)
            .HasColumnOrder(4)
            .IsRequired();

        builder.HasIndex(u => u.Phone)
        .IsUnique()
        .HasFilter("[IsDeleted] = 0");

        builder.Property(u => u.DateOfBirth)
            .HasColumnOrder(5)
            .IsRequired();

        builder.Property(u => u.Gender)
            .HasConversion<string>()
            .HasMaxLength(6)
            .HasColumnOrder(6)
            .IsRequired();

        builder.OwnsOne(u => u.Address, o =>
        {
            o.Property(a => a.BuildingNumber)
            .HasColumnName("Building_Number")
            .HasMaxLength(30)
            .HasColumnOrder(7);

            o.Property(a => a.Street)
            .HasColumnName("Street")
            .HasMaxLength(30)
            .HasColumnOrder(8);

            o.Property(a => a.City)
            .HasColumnName("City")
            .HasMaxLength(30)
            .HasColumnOrder(9);

        });

        builder.ToTable(et =>
        {
            et.HasCheckConstraint("CK_GymUser_Email",
                """
                 (
                    Email LIKE '%_@__%._%'
                        AND Email NOT LIKE '.%'
                        AND Email NOT LIKE '%.@%'
                        AND Email NOT LIKE '%..%@%'
                        AND Email NOT LIKE '%@%@%'
                        AND Email NOT LIKE '%_@%..%'
                        AND LEN(Email) <= 100
                  )
                """);
            et.HasCheckConstraint("CK_GymUser_Phone",
                """
                 (
                    (Phone LIKE '201[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
                        OR Phone LIKE '+20-1[0125]-[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
                        OR Phone LIKE '+201[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
                        OR Phone LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
                        AND LEN(Phone) <= 14
                 )
                """);
            et.HasCheckConstraint("CK_GymUser_Gender", "Gender IN ('Male','Female')");
        });
    }
}
