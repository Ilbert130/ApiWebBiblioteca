using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.DTOs
{
    public class AutorCreacionDTO
    {
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }  
    }
}
