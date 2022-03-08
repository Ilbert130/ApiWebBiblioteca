using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.Validaciones
{
    public class NumerosParesAttribute:ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int val = (int)value;

            if (val % 2 == 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("El numero no es par");
        }
    }
}
