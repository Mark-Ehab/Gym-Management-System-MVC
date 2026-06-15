using GymManagementSystem.DataAccess.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class Seeder
{
    public async static Task SeedAllAsync(GymDbContext context, string folderPath)
    {
        await PlanSeeder.SeedPlan(context, folderPath);
        var memberIds = await MemberSeeder.SeedMember(context);
        await HealthRecordSeeder.SeedHealthRecord(context, memberIds);
        await TrainerSeeder.SeedTrainer(context);
        await CategorySeeder.SeedCategory(context);
    }
}
