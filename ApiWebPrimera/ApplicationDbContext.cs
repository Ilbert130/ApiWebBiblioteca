using ApiWebPrimera.Controllers.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiWebPrimera
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //De esta manera creamos una tabla de manera automatica en la base de datos sql
        public DbSet<Autor> Autores { get; set; }
    }
}
