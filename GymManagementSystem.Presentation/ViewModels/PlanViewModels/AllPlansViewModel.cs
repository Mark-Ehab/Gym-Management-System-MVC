namespace GymManagementSystem.Presentation.ViewModels.PlanViewModels;

public sealed class AllPlansViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string Description { get; set; } = default!;
    public bool Active { get; set; }
}
