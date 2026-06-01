using GymManagementSystem.DataAccess.Enums;
using GymManagementSystem.Presentation.Validations;
using GymManagementSystem.Presentation.ViewModels.HealthRecordViewModels;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GymManagementSystem.Presentation.ViewModels.MemberViewModels;

public sealed class MemberCreateViewModel
{
    [Required(ErrorMessage = "* Name is required !")]
    [MaxLength(50,ErrorMessage ="* Name max length shall not exceed 50 characters !")]
    public string Name { get; set; } = default!;
    [Required(ErrorMessage = "* Email is required !")]
    [MaxLength(100, ErrorMessage = "* Email max length shall not exceed 100 characters !")]
    [RegularExpression(@"^(?=.{1,100}$)(?!\.)(?!.*\.@)(?!.*\.\..*@)(?!.*@.*@)(?!.*@.*\.\.)[^@\s]+@[^@\s]{2,}\.[^@\s]+$", ErrorMessage ="* Invalid Email Format !")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "* Phone number is required !")]
    [MaxLength(14, ErrorMessage = "* Phone number max length shall not exceed 14 characters !")]
    [RegularExpression(@"^(?:\+20-1[0125]-|\+?201[0125]|01[0125])\d{8}$", ErrorMessage = "* Phone number must be a valid Egyptian mobile number !")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = default!;
    [Required(ErrorMessage = "* BirthDate is required !")]
    [DateOfBirthValidationAttribute]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; } = default!;
    [Required(ErrorMessage = "* BirthDate is required !")]
    public Gender Gender { get; set; } = default!;
    [Required(ErrorMessage = "* Building Number is required !")]
    [MaxLength(30)]
    public string BuildingNumber { get; set; } = default!;
    [Required(ErrorMessage = "* Street is required !")]
    [MaxLength(30)]
    public string Street { get; set; } = default!;
    [Required(ErrorMessage = "* City is required !")]
    [MaxLength(30)]
    public string City { get; set; } = default!;
    public HealthRecordViewModel HealthRecord { get; set; } = default!;
}
