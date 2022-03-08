namespace ApiWebPrimera.Controllers.Entidades
{
    public class AutorLibro
    {
        public int LibroId { get; set; }  //ForaniKey Libro
        public int AutorId { get; set; }  //ForaniKey Autor
        public int Orden { get; set; }
        public Libro Libro { get; set; }  //Propiedad de navegacion
        public Autor Autor { get; set; }  //Propiedad de navegacion  
    }
}