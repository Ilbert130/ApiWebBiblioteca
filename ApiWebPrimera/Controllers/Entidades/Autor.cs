using ApiWebPrimera.Validaciones;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace ApiWebPrimera.Controllers.Entidades
{
    public class Autor/*:IValidatableObject*/
    {   
        [Required]
        public int Id { get; set; }
        [StringLength(maximumLength:100)]
        public string Nombre { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }



















        //[Range(18, 120, ErrorMessage = "Solo estan permitidos valors entre 18 y 120")]
        //[NumeroPrimo]
        //[NotMapped]
        //public int Edad { get; set; }
        //[CreditCard]
        //[NotMapped]
        //public string CreditCard { get; set; }
        //[Url]
        //[NotMapped]
        //public string URL { get; set; }

        //[NumerosPares]
        //public int Mayor { get; set; }
        //public int Menor { get; set; }


        ////Para que estas validaciones se cumplan tienen que cumplirse todas las que son por atributos
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    string primeraLetra = Nombre[0].ToString();

        //    if(primeraLetra != primeraLetra.ToUpper())
        //    {
        //        yield return new ValidationResult("La primera letra no es mayuscula", new string[] { nameof(Nombre) });
        //    }

        //    if (Menor > Mayor)
        //    {
        //        yield return new ValidationResult("El numero ingresado es muy elevado", new string[] { nameof(Menor) });
        //    }
        //}



        //public List<Libro> Libros { get; set; }




        //Definiendo la validacion por modelo, donde validamos un conjunto de propiedades en una funcion
        //Los resultados se almacenan en IEnumereble, que guarda una coleccion.
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!string.IsNullOrEmpty(Nombre))
        //    {
        //        var primeraLetra = Nombre[0].ToString();

        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayuscula",new string[] {nameof(Nombre)});
        //        }
        //    }

        //    if (Menor > Mayor)
        //    {
        //        yield return new ValidationResult("El valor del Menor no puede sobrepasar al valor del mayor", new string[] { nameof(Menor) });
        //    }

        //    //////////
        //    ///
        //    int count = 0;
        //    int num = Menor;

        //    for (int i = 1; i <= num; i++)
        //    {
        //        if (num % i == 0)
        //        {
        //            count++;
        //        }
        //        if (count == 3)
        //        {
        //            break;
        //        }
        //    }
        //    if (count != 2)
        //    {
        //        yield return new ValidationResult("El numero ingresado no es primo", new string[] {nameof(Menor)});
        //    }
        //}
    }
}
