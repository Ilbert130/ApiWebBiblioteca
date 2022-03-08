using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.Validaciones
{
    //Debemos heredar de la clase ValidationAttribute, la cual nos permitira definir nuestra
    //logica de validacion al sobreescribir el metodo IsValid
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        //Debemos sobre escribir el metodo IsValid para poder definir nuestra logica de validacion

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string almacen = value.ToString();
            string firstLetter = almacen[0].ToString();

            if(firstLetter!= firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra no es mayuscula");
            }

            return ValidationResult.Success;

        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    //Aqui creeamos la logica de validacion

        //    //Si es null o empty la validacion es exitoza y esto se hace para no hacer el trabajo de otro atributo
        //    //en este caso RequiredAttribute
        //    if(value == null || string.IsNullOrEmpty(value.ToString()))
        //    {
        //        return ValidationResult.Success;
        //    }

        //    //Almacenamos en una variable la primera letra de nuestra cadena
        //    var primeraLetra = value.ToString()[0].ToString();

        //    //Comparamo la letra a ver si es mayuscula
        //    if(primeraLetra != primeraLetra.ToUpper())
        //    {
        //        //Si no lo es, devolvemos esto
        //        return new ValidationResult("La primera letra debe ser mayuscula");
        //    }

        //    //Si lo es, devolvemos que es exitoso y cumplio con la regla de validacion
        //    return ValidationResult.Success;
        //}
    }
}
