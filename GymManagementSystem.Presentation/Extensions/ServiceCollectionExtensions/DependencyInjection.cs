using GymManagementSystem.Presentation.MappingProfiles;
using GymManagementSystem.Presentation.Middlewares;

namespace GymManagementSystem.Presentation.Extensions.ServiceCollectionExtensions;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services)
    {
        // Register controllers with views to the DI container.
        services.AddControllersWithViews();

        // Register Global Error Handling middleware to the DI container.
        services.AddTransient<GlobalExceptionHandler>();

        // Resgister Mapping Profiles for Presentation Layer
        services.AddAutoMapper(config => config.AddProfiles([new MemberMappingProfile(),new TrainerMappingProfile(),new PlanMappingProfile(), new SessionMappingProfile(), new AnalyticsMappingProfile()]));

        // Configure Application Cookies
        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(10);
            options.SlidingExpiration = true;
        });
    }
}
