using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Presentation.Extensions.WebApplicationExtensions;

public static class ProgramExtensions
{
    public static async Task MigrateAndSeedDatabase(this WebApplication app, CancellationToken ct = default)
    {
        /* Migrate pending migrations and Seed Database on Startup */
        using var scope = app.Services.CreateScope();
        var scopedGymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

        if (scopedGymDbContext.Database.GetPendingMigrations().Any())
            await scopedGymDbContext!.Database.MigrateAsync();

        await Seeder.SeedAllAsync(scopedGymDbContext,Path.Combine(app.Environment.ContentRootPath,"wwwroot","SeedingFiles"));
    }
}
