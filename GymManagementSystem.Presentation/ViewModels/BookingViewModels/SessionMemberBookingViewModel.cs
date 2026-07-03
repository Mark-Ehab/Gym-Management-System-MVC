namespace GymManagementSystem.Presentation.ViewModels.BookingViewModels;

public sealed class SessionMemberBookingViewModel
{
    public Guid BookingId { get; set; }
    public string MemberName { get; set; } = default!;
    public DateTime BookingDate { get; set; }
    public bool IsAttended { get; set; }

    // Computed Properties
    public string BookingDateDisplay => $"{BookingDate:MM/dd/yyyy hh:mm:ss tt}";
}
