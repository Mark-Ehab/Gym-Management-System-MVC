using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class HealthRecordSeeder
{
    public async static Task SeedHealthRecordAsync(GymDbContext context, List<Guid> memberIds)
    {
        var healthRecordDbSet = context.HealthRecords;

        /* Check if HealthRecords are already seeded */
        if (await healthRecordDbSet.AnyAsync())
            return;

        var healthRecords = new List<HealthRecord>
        {
            new()
            {
                MemberId = memberIds[0],
                Height = 178,
                Weight = 82,
                BloodType = BloodType.OPositive,
                LastUpdate = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = "No medical conditions. Goal is muscle gain."
            },

            new()
            {
                MemberId = memberIds[1],
                Height = 165,
                Weight = 58,
                BloodType = BloodType.APositive,
                LastUpdate = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = "Good cardiovascular health. Beginner in strength training."
            },

            new()
            {
                MemberId = memberIds[2],
                Height = 182,
                Weight = 88,
                BloodType = BloodType.BPositive,
                LastUpdate = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = "Recovered from a previous shoulder injury."
            },

            new()
            {
                MemberId = memberIds[3],
                Height = 168,
                Weight = 62,
                BloodType = BloodType.ANegative,
                LastUpdate = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = "Interested in weight loss and flexibility programs."
            },

            new()
            {
                MemberId = memberIds[4],
                Height = 175,
                Weight = 76,
                BloodType = BloodType.ABPositive,
                LastUpdate = DateOnly.FromDateTime(DateTime.UtcNow),
                Note = "Regular gym member with intermediate fitness level."
            }
        };

        healthRecordDbSet.AddRange(healthRecords);
        await context.SaveChangesAsync();
    }
}
