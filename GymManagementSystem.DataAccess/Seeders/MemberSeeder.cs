using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class MemberSeeder
{
    public async static Task<List<Guid>> SeedMember(GymDbContext context)
    {
        var membersDbSet = context.Members;

        /* Check if members are already seeded */
        if (await membersDbSet.AnyAsync())
            return [];

        var members = new List<Member>
        {
            new()
            {
                Name = "Ahmed Hassan",
                Email = "ahmed.hassan@gmail.com",
                Phone = "01012345678",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1998, 5, 12),
                JoinDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)),
                Address = new Address
                {
                    BuildingNumber = "12",
                    Street = "El Nasr Street",
                    City = "Cairo",
                }
            },

            new()
            {
                Name = "Sara Mohamed",
                Email = "sara.mohamed@gmail.com",
                Phone = "01198765432",
                Gender = Gender.Female,
                DateOfBirth = new DateOnly(2000, 8, 21),
                JoinDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)),
                Address = new Address
                {
                    BuildingNumber = "45",
                    Street = "Tahrir Street",
                    City = "Giza",
                }
            },

            new()
            {
                Name = "Omar Khaled",
                Email = "omar.khaled@gmail.com",
                Phone = "01245678901",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1995, 3, 14),
                JoinDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20)),
                Address = new Address
                {
                    BuildingNumber = "7A",
                    Street = "Corniche Road",
                    City = "Alexandria",
                }
            },

            new()
            {
                Name = "Mariam Adel",
                Email = "mariam.adel@gmail.com",
                Phone = "01522334455",
                Gender = Gender.Female,
                DateOfBirth = new DateOnly(1999, 11, 5),
                JoinDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6)),
                Address = new Address
                {
                    BuildingNumber = "88",
                    Street = "University Street",
                    City = "Mansoura",
                }
            },

            new()
            {
                Name = "Youssef Ali",
                Email = "youssef.ali@gmail.com",
                Phone = "01056781234",
                Gender = Gender.Male,
                DateOfBirth = new DateOnly(1997, 1, 30),
                JoinDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)),
                Address = new Address
                {
                    BuildingNumber = "3",
                    Street = "Military Road",
                    City = "Tanta",
                }
            }
        };

        membersDbSet.AddRange(members);
        await context.SaveChangesAsync();

        return [.. members.Select(m => m.Id)];
    }
}
