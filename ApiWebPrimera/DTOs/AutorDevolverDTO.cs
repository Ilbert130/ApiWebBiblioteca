using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.DTOs
{
    public class AutorDevolverDTO
    {
        [Required]
        public int Id { get; set; }
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }
    }
}
