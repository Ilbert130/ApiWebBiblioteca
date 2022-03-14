using ApiWebPrimera.Controllers.Entidades;
using ApiWebPrimera.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWebPrimera.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> GetList(int libroId)
        {
            var existe = await context.Libros.AnyAsync(l => l.Id == libroId);

            if (!existe)
            {
                return NotFound();
            }

            var comentarios = await context.Comentarios.Where(c => c.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpGet("{idComentario:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> Get(int id)
        {
            var existe = await context.Comentarios.AnyAsync(c => c.Id == id);

            if (!existe)
            {
                return BadRequest("El id del comentario ingresado no existe");

            }

            var comentario = await context.Comentarios.FirstOrDefaultAsync(c => c.Id == id);
            return mapper.Map<ComentarioDTO>(comentario);
        }


        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existe = await context.Libros.AnyAsync(l => l.Id ==libroId);

            if (!existe)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var dtoComentario = mapper.Map<ComentarioDTO>(comentario);

            //Aqui a esta ruta se le pasa el libro id y el id del comentario, por eso en la funcion anonima 
            //se pasan dos propiedades.
            return CreatedAtRoute("obtenerComentario",new {Id = comentario.Id, LiblroId = libroId},dtoComentario);
        }
    }
}
