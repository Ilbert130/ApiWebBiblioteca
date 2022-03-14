using ApiWebPrimera.Controllers.Entidades;
using ApiWebPrimera.DTOs;
using AutoMapper;
using System.Collections.Generic;

namespace ApiWebPrimera.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDevolverDTO>();
            CreateMap<Autor, AutorDevolverDTONull>()
                .ForMember(autor => autor.LibrosDevolverDTO, opciones => opciones.MapFrom(MapAutoresLibrosM));
            
            CreateMap<LibroCreacionDTO, Libro>()//configurando lo que se le va ha pasar a la propiedad AutoresLibro
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDevolverDTO>();
            CreateMap<Libro, LibroDevolverDTONull>()
                .ForMember(libro => libro.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario,ComentarioDTO>();
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if (libroCreacionDTO.AutoresIds == null)
            {
                return resultado;
            }

            //No tenemos que agregar el valor del LibroId porque 
            //Entity framework lo hace por nosotros
            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                //No agrega el id del libro por que este se agrega de manera automatica
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }

        private List<LibroDevolverDTO> MapAutoresLibrosM(Autor autor, AutorDevolverDTO autorDevolverDTO)
        {
            var resultado = new List<LibroDevolverDTO>();

            foreach(var rest in autor.AutoresLibros)
            {
                resultado.Add(new LibroDevolverDTO()
                {
                    Id = rest.LibroId,
                    Titulo = rest.Libro.Titulo
                });
            }

            return resultado;
        }

        private List<AutorDevolverDTO> MapLibroDTOAutores(Libro libro, LibroDevolverDTO libroDevolverDTO)
        {
            var resultado = new List<AutorDevolverDTO>();

            if(libro.AutoresLibros == null) { return resultado; }

            foreach(var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDevolverDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }

            return resultado;
        }

        
    }
}
