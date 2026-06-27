namespace GymManagementSystem.Presentation.ViewModels.SessionViewModel;

public sealed class SessionDetailsViewModel
{
    public string CategoryName { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public byte Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = default!;
    public int NumberOfBookingsPerSession { get; set; } = default;

    // Computed Properties
    public int AvailableSlots
        => Capacity - NumberOfBookingsPerSession;
    public TimeSpan Duration
        => EndDate - StartDate;
}
