using System.ComponentModel.DataAnnotations;

namespace ApiWebPrimera.DTOs
{
    public class EditarAdminDTO
    {   
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
