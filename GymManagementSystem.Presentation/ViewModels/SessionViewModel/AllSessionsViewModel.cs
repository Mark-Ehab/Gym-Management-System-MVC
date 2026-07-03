namespace GymManagementSystem.Presentation.ViewModels.SessionViewModel;

public sealed class AllSessionsViewModel
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string TrainerName { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public byte Capacity { get; set; }
    public string Status { get; set; } = default!;
    public int NumberOfBookingsPerSession { get; set; } 

    // Computed Properties
    public string DateDisplay => $"{StartDate:MMM dd,yyyy}";
    public string TimeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";
    public TimeSpan Duration => EndDate - StartDate;
    public int AvailableSlots => Capacity - NumberOfBookingsPerSession;
}
