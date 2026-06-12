using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.Presentation.Validations;
using GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GymManagementSystem.Presentation.ViewModels.MemberViewModels;

public sealed class MemberCreateViewModel
{
    [Required(ErrorMessage = "* Name is required !")]
    [StringLength(50,ErrorMessage ="* Name max length shall not exceed 50 characters !")]
    [Display(Name = "Name")]
    public string Name { get; set; } = default!;
    [Required(ErrorMessage = "* Email is required !")]
    [StringLength(100, ErrorMessage = "* Email max length shall not exceed 100 characters !")]
    [RegularExpression(@"^(?=.{1,100}$)(?!\.)(?!.*\.@)(?!.*\.\..*@)(?!.*@.*@)(?!.*@.*\.\.)[^@\s]+@[^@\s]{2,}\.[^@\s]+$", ErrorMessage ="* Invalid Email Format !")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "* Phone number is required !")]
    [StringLength(14, ErrorMessage = "* Phone number max length shall not exceed 14 characters !")]
    [RegularExpression(@"^(?:\+20-1[0125]-|\+?201[0125]|01[0125])\d{8}$", ErrorMessage = "* Phone number must be a valid Egyptian mobile number !")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = default!;
    [Required(ErrorMessage = "* BirthDate is required !")]
    [DateOfBirthValidationAttribute]
    [DataType(DataType.Date)]
    [Display(Name = "DateOfBirth")]
    public DateOnly DateOfBirth { get; set; } = default!;
    [Required(ErrorMessage = "* BirthDate is required !")]
    [Display(Name = "Gender")]
    public Gender Gender { get; set; } = default!;
    [Required(ErrorMessage = "* Building Number is required !")]
    [StringLength( 30, ErrorMessage = "* Building Number max length shall not exceed 30 characters !")]
    [Display(Name = "BuildingNumber")]
    public string BuildingNumber { get; set; } = default!;
    [Required(ErrorMessage = "* Street is required !")]
    [StringLength(30, ErrorMessage = "* Street max length shall not exceed 30 characters !")]
    [Display(Name = "Street")]
    public string Street { get; set; } = default!;
    [Required(ErrorMessage = "* City is required !")]
    [StringLength(30,ErrorMessage = "* City max length shall not exceed 30 characters !")]
    [Display(Name = "City")]
    public string City { get; set; } = default!;
    public HealthRecordViewModel HealthRecord { get; set; } = default!;
}
