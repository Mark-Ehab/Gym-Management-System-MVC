using GymManagementSystem.Presentation.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.PlanViewModels;

public sealed class PlanToBeEditedViewModel
{
    [Required(ErrorMessage = "* Plan name is required !")]
    [ReadOnly(true)]
    [AllowedValues("Annual", "Standard", "Premium" ,"Basic",
        ErrorMessage = "* Plan name shall be either of the following values (Annual, Standard, Premium ,Basic) !")]
    [Display(Name = "Plan Name")]
    public string Name { get; set; } = default!;
    [Required(ErrorMessage = "* Plan price is required !")]
    [PlanPriceValidationAttribute(10,2)]
    [DataType(DataType.Currency)]
    [Display(Name = "Price (EGP)")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "* Plan duration days is required !")]
    [Range(1,365,ErrorMessage = "* Plan duration days must be between 1 and 365 !")]
    [Display(Name = "Duration (Days)")]
    public int DurationDays { get; set; }
    [Required(ErrorMessage = "* Plan description is required !")]
    [Display(Name = "Description")]
    public string Description { get; set; } = default!;
}
