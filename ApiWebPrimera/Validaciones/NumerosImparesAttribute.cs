using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.Validaciones
{
    public class NumerosImparesAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int val = (int)value;

            if (val % 2 == 0)
            {
                return new ValidationResult("ERROR: El numero es par");
            }

            return ValidationResult.Success;
        }
    }
}
