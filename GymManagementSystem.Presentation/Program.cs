using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.Extensions.ServiceCollectionExtensions;
using GymManagementSystem.BusinessLogic.Services;
using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Extensions.ServiceCollectionExtensions;
using GymManagementSystem.DataAccess.Interceptors;
using GymManagementSystem.DataAccess.Repositories.Classes;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.Seeders;
using GymManagementSystem.Presentation.Extensions.ServiceCollectionExtensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register Presentation layer services to the DI container.
builder.Services.AddPresentation();

// Register Business Logic layer services to the DI container.
builder.Services.AddBusinessLogic();

// Register Data Access layer services to the DI container.
builder.Services.AddDataAccess(builder.Configuration);

var app = builder.Build();

/* Seed Database on Startup */
using var scope = app.Services.CreateScope();
var scopedGymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
await scopedGymDbContext!.Database.MigrateAsync();
await Seeder.SeedAllAsync(scopedGymDbContext);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
