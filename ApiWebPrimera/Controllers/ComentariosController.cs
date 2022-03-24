using ApiWebPrimera.Controllers.Entidades;
using ApiWebPrimera.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var existe = await context.Libros.AnyAsync(l => l.Id ==libroId);

            if (!existe)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var dtoComentario = mapper.Map<ComentarioDTO>(comentario);

            //Aqui a esta ruta se le pasa el libro id y el id del comentario, por eso en la funcion anonima 
            //se pasan dos propiedades.

            //return CreatedAtRoute("obtenerComentario",new {Id = comentario.Id, LiblroId = libroId},dtoComentario);
            return Ok(dtoComentario);
        }
    }
}
