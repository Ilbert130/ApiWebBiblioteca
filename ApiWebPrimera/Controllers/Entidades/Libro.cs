using ApiWebPrimera.Validaciones;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.Controllers.Entidades
{
    public class Libro
    {
        //[Required(ErrorMessage = "Este campo es requerido")]
        [Required]
        public int Id { get; set; }
        //[StringLength(maximumLength:60, ErrorMessage ="Has superado la cantidad de caracteres permitidos")]
        [Required]
        [StringLength(maximumLength: 100)]
        public string? Titulo { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }
        //[Range(1,200,ErrorMessage ="Has ingresado un valor que supera al permitido")]
        //[NumeroPrimo]
        /*public int AutorId { get; set; }*/
        //public Autor? Autor { get; set; }
    }
}
