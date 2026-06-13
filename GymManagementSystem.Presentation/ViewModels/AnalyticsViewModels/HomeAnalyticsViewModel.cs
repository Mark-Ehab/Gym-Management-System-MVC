namespace GymManagementSystem.Presentation.ViewModels.AnalyticsViewModels;

public sealed class HomeAnalyticsViewModel
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int Trainers { get; set; }
    public int UpcomingSessions { get; set; }
    public int OngoingSessions { get; set; }
    public int CompletedSessions { get; set; }
}
