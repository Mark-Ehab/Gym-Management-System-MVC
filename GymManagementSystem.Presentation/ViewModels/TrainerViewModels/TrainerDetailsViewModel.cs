using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.Presentation.Validations;
using GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.TrainerViewModels;

public sealed class TrainerDetailsViewModel
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string Specialization { get; set; } = default!;
    public string Address { get; set; } = default!;
}
