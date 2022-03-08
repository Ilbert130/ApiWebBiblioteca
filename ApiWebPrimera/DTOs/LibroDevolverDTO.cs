using ApiWebPrimera.Controllers.Entidades;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.DTOs
{
    public class LibroDevolverDTO
    {
        //[Required(ErrorMessage = "Este campo es requerido")]
        [Required]
        public int Id { get; set; }
        //[StringLength(maximumLength:60, ErrorMessage ="Has superado la cantidad de caracteres permitidos")]
        [StringLength(maximumLength: 100)]
        public string? Titulo { get; set; }
        //public List<Comentario> Comentarios { get; set; }
    }
}
