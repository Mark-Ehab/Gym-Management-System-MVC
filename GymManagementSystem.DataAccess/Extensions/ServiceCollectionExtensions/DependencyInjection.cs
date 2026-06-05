using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Interceptors;
using GymManagementSystem.DataAccess.Repositories.Classes;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Class;
using GymManagementSystem.DataAccess.UoW.Contract;
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
        // Register Unit of Work to DI Container
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

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
