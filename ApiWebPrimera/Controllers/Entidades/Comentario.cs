namespace ApiWebPrimera.Controllers.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        //Propiedad de navegacion, que me permite optener la data de esa entidad 
        //Relacionada con este comentario, basicamente es un JOIN
        public Libro Libro { get; set; }
    }
}
