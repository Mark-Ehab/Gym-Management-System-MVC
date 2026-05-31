namespace GymManagementSystem.Presentation.ViewModels.MemberViewModels;

public sealed class AllMembersViewModel
{
    public Guid Id { get; set; }
    public string? Photo { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Gender { get; set; } = default!;
}
