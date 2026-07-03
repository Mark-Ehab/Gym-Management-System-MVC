using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.Validations;

public sealed class DateOfBirthValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dateOfBirth = (DateOnly) value!;
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        if(dateOfBirth >= currentDate)
        {
            return new ValidationResult("* Date of birth shall be earlier than today !");
        }

        if(dateOfBirth.AddYears(18) > currentDate)
        {
            return new ValidationResult("* Age must be at least 18 years old !");
        }

        return ValidationResult.Success;
    }
}
