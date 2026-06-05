namespace GymManagementSystem.Presentation.Extensions.ServiceCollectionExtensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Register controllers with views to the DI container.
        services.AddControllersWithViews();

        return services;
    }
}
