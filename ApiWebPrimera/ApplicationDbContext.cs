using ApiWebPrimera.Controllers.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiWebPrimera
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Con este metod creamos una llave compuesta para la relacion muchos a muchos
        //La tabla AutorLibro
        //Es necesario mantener la referencia al continido base de este metodo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Es importante tener esta referencia aqui si se rescribe el codigo
            base.OnModelCreating(modelBuilder);

            //Definimos que composicion sera la llave primaria de AutorLibro
            modelBuilder.Entity<AutorLibro>().HasKey(al => new { al.AutorId, al.LibroId });
        }

        //De esta manera creamos una tabla de manera automatica en la base de datos sql
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set;}
    }
}
