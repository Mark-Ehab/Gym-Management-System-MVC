using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.MembershipViewModels;

public sealed class CreateMembershipViewModel
{
    [Required(ErrorMessage = "Memeber is required !")]
    [Display(Name = "Member")]
    public Guid Member { get; set; }
    [Required(ErrorMessage = "Plan is required !")]
    [Display(Name = "Plan")]
    public Guid Plan { get; set; }
}
