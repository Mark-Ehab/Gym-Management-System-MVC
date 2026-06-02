using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Interceptors;
using GymManagementSystem.DataAccess.Repositories.Classes;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Extensions.ServiceCollectionExtensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        // Register any instance from GenericRepository<> and implementing IGenericRepository<> to DI Container
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Register GymDbContext to DI Container
        services.AddDbContext<GymDbContext>(options =>

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(new SoftDeleteInterceptor(), new AuditColumnsInterceptor())
        );

        // Register instances of types SoftDeleteInterceptor and AuditColumnsInterceptor as singleton to DI container
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<AuditColumnsInterceptor>();

        return services;
    }
}
