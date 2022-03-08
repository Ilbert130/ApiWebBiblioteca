using ApiWebPrimera.Controllers.Entidades;
using ApiWebPrimera.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWebPrimera.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<LibrosController> logger;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, ILogger<LibrosController> logger,IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<LibroDevolverDTO>>> Get()
        {
            var libro = await context.Libros.ToListAsync();
            return mapper.Map<List<LibroDevolverDTO>>(libro);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDevolverDTONull>> Get(int id)
        {
            var libro = await context.Libros
                .Include(l => l.AutoresLibros)
                .ThenInclude(al => al.Autor)
                .FirstOrDefaultAsync(l => l.Id == id);
            return mapper.Map<LibroDevolverDTONull>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroDTO)
        {
            if(libroDTO.AutoresIds == null || libroDTO.AutoresIds.Count==0)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            //De esta manera verificamos que los id enviado exista en la tabla id de autores
            //Comparandolos con los de la tabla de autores
            var autoresIds = await context.Autores.Where(a => libroDTO.AutoresIds.Contains(a.Id)).Select(x => x.Id).ToListAsync();


            //Si el resultado de la validacion es diferente a la cantidad de id pasados por parametro
            //es porque hay un id que no existe en la tabla autor
            if(libroDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviado");
            }

            var libro = mapper.Map<Libro>(libroDTO);

            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
