using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.BookingViewModels;

public sealed class CreateBookingViewModel
{
    [Required(ErrorMessage ="Member is required !")]
    [Display(Name = "Member")]
    public Guid MemberId { get; set; }
}
