using GymManagementSystem.DataAccess.Data.Contexts;
using GymManagementSystem.DataAccess.Interceptors;
using GymManagementSystem.DataAccess.Models.IdentityModels;
using GymManagementSystem.DataAccess.Repositories.Classes;
using GymManagementSystem.DataAccess.Repositories.Contracts;
using GymManagementSystem.DataAccess.UoW.Class;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Extensions.ServiceCollectionExtensions;

public static class DependencyInjection
{
    public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Unit of Work to DI Container
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

        // Register Session Repository to DI Container
        services.AddScoped(typeof(ISessionRepository), typeof(SessionRepository));

        // Register any instance from GenericRepository<> and implementing IGenericRepository<> to DI Container
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Register instances of types SoftDeleteInterceptor and AuditColumnsInterceptor as singleton to DI container
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<AuditColumnsInterceptor>();

        // Register GymDbContext to DI Container
        services.AddDbContext<GymDbContext>(options =>

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(new SoftDeleteInterceptor(), new AuditColumnsInterceptor())
        );

        // Register Identity Services
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            // Password Configs
            options.Password.RequiredLength = 8;

            // User Configs
            options.User.RequireUniqueEmail = true;

            // Lockout Configs
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
            .AddEntityFrameworkStores<GymDbContext>();   
    }
}
