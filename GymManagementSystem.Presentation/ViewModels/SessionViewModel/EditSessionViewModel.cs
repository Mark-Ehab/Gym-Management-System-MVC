using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.SessionViewModel;

public sealed class EditSessionViewModel : IValidatableObject
{
    [Required(ErrorMessage = "* Trainer is required !")]
    [Display(Name = "Trainer")]
    public Guid TrainerId { get; set; }
    [Required(ErrorMessage = "* Description is required !")]
    [StringLength(400, ErrorMessage = "* Description maximum length is 400 characters !")]
    [Display(Name = "Description")]
    public string Description { get; set; } = default!;
    [Required(ErrorMessage = "* Start Date & Time is required !")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Start Date & Time")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "* End Date & Time is required !")]
    [DataType(DataType.DateTime)]
    [Display(Name = "End Date & Time")]
    public DateTime EndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        /* Check if EndDate is always after StartDate */
        if (StartDate > EndDate)
            yield return new("* Start date must be before End date !", [nameof(StartDate), nameof(EndDate)]);

        /* Check that Start Date dosn't equal Enddate */
        if (StartDate == EndDate)
            yield return new("* Start date shall not equal End date !", [nameof(StartDate), nameof(EndDate)]);

        /* Check the Start Date is in the future */
        if (StartDate < DateTime.Now)
            yield return new("* Start date must be in the future !", [nameof(StartDate)]);
    }
}
