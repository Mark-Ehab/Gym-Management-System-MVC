using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.DTOs.AnalyticsDTOs;

public sealed class HomeAnalyticsDTO
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int Trainers { get; set; }
    public int UpcomingSessions { get; set; }
    public int OngoingSessions { get; set; }
    public int CompletedSessions { get; set; }
}
