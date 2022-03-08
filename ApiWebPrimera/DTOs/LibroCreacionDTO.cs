using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.DTOs
{
    public class LibroCreacionDTO
    {
        //[StringLength(maximumLength:60, ErrorMessage ="Has superado la cantidad de caracteres permitidos")]
        [StringLength(maximumLength: 100)]
        public string? Titulo { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
