using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Models.IdentityModels;
using GymManagementSystem.DataAccess.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Presentation.Extensions.WebApplicationExtensions;

public static class ProgramExtensions
{
    public static async Task MigrateAndSeedDatabase(this WebApplication app, IConfiguration config, CancellationToken ct = default)
    {
        // Create a service scope
        using var scope = app.Services.CreateScope();

        // Get required scoped services
        var scopedGymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        var scopedUserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var scopedRoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Check if there are any pending migrations
        if (scopedGymDbContext.Database.GetPendingMigrations().Any())
            // Update database Pending Migrations
            await scopedGymDbContext!.Database.MigrateAsync(ct);        

        // Seed Database on Startup (Including Business and Identity tables seeding)
        await Seeder.SeedAllAsync(scopedGymDbContext,Path.Combine(app.Environment.ContentRootPath,"wwwroot","SeedingFiles"),scopedUserManager,scopedRoleManager,config);
    }
}
