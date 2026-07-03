using GymManagementSystem.Presentation.Validations;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;

public sealed class HealthRecordViewModel
{
    [Required(ErrorMessage = "* Height is required !")]
    [HeightValidation(5, 2)]
    [Display(Name = "Height")]
    public decimal Height { get; set; }
    [Required(ErrorMessage = "* Weight is required !")]
    [WeightValidation(5, 2)]
    [Display(Name = "Weight")]
    public decimal Weight { get; set; }
    [Required(ErrorMessage = "* Blood Type is required !")]
    [Display(Name = "Blood Type")]
    public string BloodType { get; set; } = default!;
    [StringLength(100,ErrorMessage = "Note shall not exceed 100 characters !")]
    [Display(Name = "Note")]
    public string? Note { get; set; }
}
