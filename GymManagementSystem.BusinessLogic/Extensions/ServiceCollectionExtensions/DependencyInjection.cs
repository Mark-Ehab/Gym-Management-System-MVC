using GymManagementSystem.BusinessLogic.Contracts.AttachmentService;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.MappingProfiles;
using GymManagementSystem.BusinessLogic.Services.AttachmentService;
using GymManagementSystem.BusinessLogic.Services.BusinessServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Extensions.ServiceCollectionExtensions;

public static class DependencyInjection
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        // Register business logic services to the DI container.
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddAutoMapper(config => config.AddProfiles([new MemberMappingProfile(),new TrainerMappingProfile(),new PlanMappingProfile(),new SessionMappingProfile()]));
    }
}