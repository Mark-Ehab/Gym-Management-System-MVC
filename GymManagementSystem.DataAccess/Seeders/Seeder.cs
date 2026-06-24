using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class Seeder
{
    public async static Task SeedAllAsync(GymDbContext context,
        string folderPath,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration config)
    {
        await PlanSeeder.SeedPlanAsync(context, folderPath);
        var memberIds = await MemberSeeder.SeedMemberAsync(context);
        await HealthRecordSeeder.SeedHealthRecordAsync(context, memberIds);
        await TrainerSeeder.SeedTrainerAsync(context);
        await CategorySeeder.SeedCategoryAsync(context);
        await IdentitySeeder.SeedIdentityAsync(userManager,roleManager,config);
    }
}
