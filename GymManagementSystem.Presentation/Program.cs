using GymManagementSystem.BusinessLogic.Contracts.Services;
using GymManagementSystem.BusinessLogic.Services;
using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Interceptors;
using GymManagementSystem.DataAccess.Repositories.Classes;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPlanService,PlanService>();
builder.Services.AddScoped<IMemberService,MemberService>();
builder.Services.AddScoped<ITrainerService,TrainerService>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

// Register GymDbContext to DI Container
builder.Services.AddDbContext<GymDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddInterceptors(new SoftDeleteInterceptor(),new AuditColumnsInterceptor())
);

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
