using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace GymManagementSystem.DataAccess.Seeders;

public static class PlanSeeder
{
    public async static Task SeedPlanAsync(GymDbContext context, string folderPath)
    {
        var plansDbSet = context.Plans;

        /* Check if plans are already seeded */
        if (await plansDbSet.AnyAsync())
            return;

        try
        {
            /* Seed plans table from external Json file */
            var planSeedingFilePath = Path.Combine(folderPath, "plans.json");

            /* Ckeck if this file exists */
            if (!File.Exists(planSeedingFilePath))
                throw new FileNotFoundException($"The file whose path is {planSeedingFilePath} doesn't exist");

            var plansData = File.ReadAllText(planSeedingFilePath);

            var plans = JsonSerializer.Deserialize<List<Plan>>(plansData,new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            }) ?? [];

                plansDbSet.AddRange(plans);
            await context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }
}
