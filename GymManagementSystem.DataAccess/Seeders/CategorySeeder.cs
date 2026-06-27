using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Seeders;

public static class CategorySeeder
{
    public async static Task SeedCategoryAsync(GymDbContext context)
    {
        var categoryDbSet = context.Categories;

        /* Check if categories are already seeded */
        if (await categoryDbSet.AnyAsync())
            return;

        var categories = new List<Category>() 
        { 
            new () { Name = "GeneralFitness" },
            new () { Name = "Yoga" },
            new () { Name = "Boxing" },
            new () { Name = "CrossFit" },
            new () { Name = "Nutrition" },
            new () { Name = "StrengthTrainig" }
        };

        categoryDbSet.AddRange(categories);
        await context.SaveChangesAsync();
    }
}
