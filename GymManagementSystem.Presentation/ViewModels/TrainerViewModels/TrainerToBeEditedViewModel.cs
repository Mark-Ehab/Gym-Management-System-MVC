using GymManagementSystem.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.TrainerViewModels;

public sealed class TrainerToBeEditedViewModel
{
    public string? Name { get; set; }
    [Required(ErrorMessage = "* Email is required !")]
    [MaxLength(100, ErrorMessage = "* Email max length shall not exceed 100 characters !")]
    [RegularExpression(@"^(?=.{1,100}$)(?!\.)(?!.*\.@)(?!.*\.\..*@)(?!.*@.*@)(?!.*@.*\.\.)[^@\s]+@[^@\s]{2,}\.[^@\s]+$", ErrorMessage = "* Invalid Email Format !")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "* Phone number is required !")]
    [MaxLength(14, ErrorMessage = "* Phone number max length shall not exceed 14 characters !")]
    [RegularExpression(@"^(?:\+20-1[0125]-|\+?201[0125]|01[0125])\d{8}$", ErrorMessage = "* Phone number must be a valid Egyptian mobile number !")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = default!;
    [Required(ErrorMessage = "* Building Number is required !")]
    [StringLength(30, ErrorMessage = "* Building Number max length shall not exceed 30 characters !")]
    [Display(Name = "BuildingNumber")]
    public string BuildingNumber { get; set; } = default!;
    [Required(ErrorMessage = "* Street is required !")]
    [StringLength(30, ErrorMessage = "* Street max length shall not exceed 30 characters !")]
    [Display(Name = "Street")]
    public string Street { get; set; } = default!;
    [Required(ErrorMessage = "* City is required !")]
    [StringLength(30, ErrorMessage = "* City max length shall not exceed 30 characters !")]
    [Display(Name = "City")]
    public string City { get; set; } = default!;
    [Required(ErrorMessage = "* Specialization is required !")]
    public Speciality Specialties { get; set; } = default!;
}
