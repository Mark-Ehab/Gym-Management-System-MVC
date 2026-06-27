namespace GymManagementSystem.Presentation.ViewModels.MembershipViewModels;

public sealed class AllMembershipsViewModel
{
    public Guid MemberId { get; set; } 
    public string MemberName { get; set; } = default!;
    public string PlanName { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    // Computed Properties
    public string StartDateDisplay => $"{StartDate:MMM dd, yyyy}";
    public string EndDateDisplay => $"{EndDate:MMM dd, yyyy}";
}
