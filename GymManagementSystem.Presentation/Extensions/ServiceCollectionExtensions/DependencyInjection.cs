using GymManagementSystem.Presentation.MappingProfiles;

namespace GymManagementSystem.Presentation.Extensions.ServiceCollectionExtensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Register controllers with views to the DI container.
        services.AddControllersWithViews();

        // Resgister Mapping Profiles for Presentation Layer
        services.AddAutoMapper(config => config.AddProfiles([new MemberMappingProfile(),new TrainerMappingProfile(),new PlanMappingProfile()]));

        return services;
    }
}
