using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Presentation.Validations;

public sealed class PlanPriceValidationAttribute : ValidationAttribute
{
    private int _pricePrecision;
    private int _priceScale;

    public PlanPriceValidationAttribute(int precision, int scale)
    {
        _pricePrecision = precision;
        _priceScale = scale;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var priceParts = value!.ToString().Split('.');

        var integerDigitsLength = priceParts[0].Length;
        var decimalDigitsLength = priceParts[1].Length;
        var totalDigitsLength = integerDigitsLength + decimalDigitsLength;

        if (totalDigitsLength > _pricePrecision)
            return new ValidationResult($"* Plan price precision shall be up tp {_pricePrecision} digits only !");

        if(decimalDigitsLength > _priceScale)
            return new ValidationResult($"* Plan price scale shall be up to {_priceScale} digits only !");

        return ValidationResult.Success;
    }
}
