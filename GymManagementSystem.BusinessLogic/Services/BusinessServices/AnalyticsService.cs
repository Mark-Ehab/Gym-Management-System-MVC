using GymManagementSystem.BusinessLogic.BusinessSpecifictions.MembershipSpecifications;
using GymManagementSystem.BusinessLogic.BusinessSpecifictions.SessionSpecifications;
using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts.BusinessServices;
using GymManagementSystem.BusinessLogic.DTOs.AnalyticsDTOs;
using GymManagementSystem.DataAccess.Models;
using GymManagementSystem.DataAccess.UoW.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.BusinessServices;

public sealed class AnalyticsService : IAnalyticsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(IUnitOfWork unitOfWork, ILogger<AnalyticsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<HomeAnalyticsDTO>> GetHomeAnalyticsAsync(CancellationToken ct)
    {
        var totalMembers = await _unitOfWork.GetGenericRepository<Member>().CountAsync(ct: ct);
        var activeMembers = await _unitOfWork.GetGenericRepository<Membership>().CountAsync(new ActiveMembershipsSpecification(),ct);
        var totalTrainers = await _unitOfWork.GetGenericRepository<Trainer>().CountAsync(ct: ct);
        var upcomingSessions = await _unitOfWork.GetGenericRepository<Session>().CountAsync(new UpcomingSessionsSpecification(), ct: ct);
        var ongoingSessions = await _unitOfWork.GetGenericRepository<Session>().CountAsync(new OngoingSessionsSpecification(), ct: ct);
        var completedSessions = await _unitOfWork.GetGenericRepository<Session>().CountAsync(new CompletedSessionsSpecification(),ct: ct);

        _logger.LogInformation("Dashboard analytics are retrieved successfully");
        return Result<HomeAnalyticsDTO>.Success(new()
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            Trainers = totalTrainers,
            UpcomingSessions = upcomingSessions,
            CompletedSessions = completedSessions,
            OngoingSessions = ongoingSessions
        });
    }

}
