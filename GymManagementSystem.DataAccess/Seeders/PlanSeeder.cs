using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class PlanSeeder
{
    public async static Task SeedPlan(GymDbContext context)
    {
        var plansDbSet = context.Plans;

        /* Check if plans are already seeded */
        if (await plansDbSet.AnyAsync())
            return;

        var plans = new List<Plan>
        {
            new()
            {
                Name = "Basic",
                Description = "Access to gym equipment only",
                DurationDays = 30,
                Price = 499,
                IsActive = true
            },

            new()
            {
                Name = "Standard",
                Description = "Gym access with 5 group sessions",
                DurationDays = 90,
                Price = 1299,
                IsActive = true
            },

            new()
            {
                Name = "Premium",
                Description = "Unlimited gym and personal trainer sessions",
                DurationDays = 180,
                Price = 2499,
                IsActive = true
            },

            new()
            {
                Name = "Annual",
                Description = "Full VIP membership with nutrition follow-up",
                DurationDays = 365,
                Price = 4999,
                IsActive = true
            }
        };

        plansDbSet.AddRange(plans);
        await context.SaveChangesAsync();
    }
}
