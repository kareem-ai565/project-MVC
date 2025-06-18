using System.ComponentModel.DataAnnotations;

namespace project.ValidationAttributes
{
    namespace project.Attributes
    {
        public class DivisibleByAttribute : ValidationAttribute
        {
            private readonly int _divisor;

            public DivisibleByAttribute(int divisor)
            {
                _divisor = divisor;
            }

            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value == null) return ValidationResult.Success;

                if (value is int intValue && intValue % _divisor == 0)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage ?? $"Value must be divisible by {_divisor}.");
            }
        }
    }

}
