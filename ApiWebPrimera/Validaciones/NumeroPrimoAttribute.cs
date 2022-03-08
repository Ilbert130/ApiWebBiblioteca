using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.Validaciones
{
    public class NumeroPrimoAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int count = 0;
            int num = (int)value;

            for(int i = 1; i <= num; i++)
            {
                if (num % i == 0)
                {
                    count++;
                }
                if (count == 3)
                {
                    break;
                }
            }
            if (count == 2)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("El numero ingresado no es primo");
            }
        }
    }
}
