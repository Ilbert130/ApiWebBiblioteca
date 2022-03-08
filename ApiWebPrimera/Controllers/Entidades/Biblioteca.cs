using ApiWebPrimera.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace ApiWebPrimera.Controllers.Entidades
{
    public class Biblioteca
    {
        //[Required(ErrorMessage ="La propiedad tiene que ser obligatoria")]
        //[Key]
        public int Id { get; set; }
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        
    }
}
