using GymManagementSystem.Presentation.Validations;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;

public sealed class HealthRecordViewModel
{
    [Required(ErrorMessage = "* Height is required !")]
    [HeightValidation(5, 2)]
    public decimal Height { get; set; }
    [Required(ErrorMessage = "* Weight is required !")]
    [WeightValidation(5, 2)]
    public decimal Weight { get; set; }
    [Required(ErrorMessage = "* Bllod Type is required !")]
    public string BloodType { get; set; } = default!;
    [MaxLength(100)]
    public string? Note { get; set; }
}
