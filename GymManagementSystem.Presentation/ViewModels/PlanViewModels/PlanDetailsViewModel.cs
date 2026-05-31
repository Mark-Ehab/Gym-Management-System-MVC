namespace GymManagementSystem.Presentation.ViewModels.PlanViewModels;

public sealed class PlanDetailsViewModel
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
}
