using GymManagementSystem.DataAccess.Enums;

namespace GymManagementSystem.Presentation.ViewModels.TrainerViewModels;

public sealed class AllTrainersViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Specialization { get; set; } = default!;
}
