namespace GymManagementSystem.Presentation.ViewModels.MemberViewModels;

public sealed class MemberDetailsViewModel
{
    public string? Photo { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? PlanName { get; set; }
    public string? MembershipStartDate { get; set; }
    public string? MembershipEndDate { get; set; }
}
