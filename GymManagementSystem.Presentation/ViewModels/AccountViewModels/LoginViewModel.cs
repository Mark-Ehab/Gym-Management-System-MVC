using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.ViewModels.AccountViewModels;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "* Email is required !")]
    [StringLength(100, ErrorMessage = "* Email max length shall not exceed 100 characters !")]
    [RegularExpression(@"^(?=.{1,100}$)(?!\.)(?!.*\.@)(?!.*\.\..*@)(?!.*@.*@)(?!.*@.*\.\.)[^@\s]+@[^@\s]{2,}\.[^@\s]+$", ErrorMessage = "* Invalid Email Format !")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "* Password is required !")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = default!;
    public string? ReturnUrl { get; set; } = default!;
    public bool RememberMe { get; set; } 
}
