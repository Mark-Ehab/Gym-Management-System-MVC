using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.DTOs.AnalyticsDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.BusinessServices;

public interface IAnalyticsService
{
    Task<Result<HomeAnalyticsDTO>> GetHomeAnalyticsAsync(CancellationToken ct);
}
