using GymManagementSystem.BusinessLogic.DTOs.AnalyticsDTOs;
using GymManagementSystem.BusinessLogic.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.Services;

public interface IAnalyticsService
{
    Task<Result<HomeAnalyticsDTO>> GetHomeAnalyticsAsync(CancellationToken ct);
}
