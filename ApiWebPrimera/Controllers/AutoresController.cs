using ApiWebPrimera.Controllers.Entidades;
using ApiWebPrimera.DTOs;
using ApiWebPrimera.Filtros;
using ApiWebPrimera.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWebPrimera.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        //private readonly IServices services;
        //private readonly ServicioTransient servicioTransient;
        //private readonly ServicioScoped servicioScoped;
        //private readonly ServicioSingleton servicioSingleton;
        //private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration/*, IServices services, ServicioTransient servicioTransient
            ,ServicioScoped servicioScoped, ServicioSingleton servicioSingleton, ILogger<AutoresController> logger*/)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            //this.services = services;
            //this.servicioTransient = servicioTransient;
            //this.servicioScoped = servicioScoped;
            //this.servicioSingleton = servicioSingleton;
            //this.logger = logger;
        }

        //[HttpGet("GUID")]
        //[ServiceFilter(typeof(MiFltroDeAccion))] //Aqui estamos usando el filtro que creamos
        //[ResponseCache(Duration =10)] //Filtro para guardar informacion en cache por 10 segundo
        //public ActionResult ObtenerGuis()
        //{
        //    return Ok(new {
        //        AutoresControllerTransient = servicioTransient.Guid,
        //        ServicioA_Transient = services.ObtenerTransient(),

        //        AutoresControllerScoped = servicioScoped.Guid,
        //        ServicioA_Scoped = services.ObtenerScoped(),

        //        AutoresControllerSingleton = servicioSingleton.Guid,
        //        ServicioA_Singleton = services.ObtenerSingleton()
        //    });
        //}

        [HttpGet("configuraciones")]
        public ActionResult<string> GetConfiguracion()
        {
            return configuration["ConnectionStrings:defaultConnection"];
        }

        //[HttpGet]
        [HttpGet("listado")] // api/autores/listado
        //[Authorize]
        public async Task<ActionResult<List<AutorDevolverDTO>>> Get()
        {
            //throw new NotImplementedException();
            //logger.LogInformation("Estamos obteniendo los autores");
            //logger.LogWarning("Estamos obteniendo un mensaje de warning");
            var val = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDevolverDTO>>(val);
        
        }

        [HttpGet("ilbert/{id}",Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDevolverDTONull>> ObtenerAutor(int id)
        {
            var autor = await context.Autores
                .Include(a => a.AutoresLibros)
                .ThenInclude(al => al.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return mapper.Map<AutorDevolverDTONull>(autor);
        }

        //[HttpGet("include")]
        //public async Task<ActionResult<List<Autor>>> GetValAut()
        //{
        //    return await context.Autores.Include(x => x.Libros).ToListAsync();
        //}

        [HttpGet("primero")] // api/autores/primero
        public async Task<ActionResult<AutorDevolverDTO>> PrimerAutor([FromRoute] int miValor)
        {
            var valor = await context.Autores.FirstOrDefaultAsync();
            return mapper.Map<AutorDevolverDTO>(valor);
        }

        [HttpGet("{id:int}")]// api/autores/id //aqui definimos una variable para la peticion del autor por id
        public async Task<ActionResult<Autor>> GetAutorId(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(e => e.Id == id);

            if (autor == null)
            {
                return NotFound();//Retorna un 404
            }

            return autor;
        }

        [HttpGet("ruta/{nombre}")] // api/autores?nombre=Ilbert%20Castillo
        public async Task<ActionResult<Autor>> GetListAut([FromQuery]string nombre)
        {
            var valor = await context.Autores.AnyAsync(d => d.Nombre == nombre);
            if (!valor)
            {
                return BadRequest();
            }

            return await context.Autores.FirstAsync(d => d.Nombre == nombre);
        }

        //[HttpGet("prueba")]
        //public IActionResult GetlistAut()
        //{
        //    var autor = context.Autores.Include(x => x.Libros).ToList();

        //    return Ok(autor);
        //}



        //Aqui estamos creando una validacion en el controlador
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]AutorCreacionDTO autorCreacionDTO)
        {
            var existe = await context.Autores.AnyAsync(d =>d.Nombre == autorCreacionDTO.Nombre);

            if (existe)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDevolverDTO>(autor);

            return CreatedAtRoute("obtenerAutor",new {id = autor.Id}, autorDTO);
        }

        [HttpPut("{id:int}")] // api/autores/1
        public async Task<ActionResult> Put([FromBody]AutorCreacionDTO autorCreacionDTO, int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent(); //204 
        }

        [HttpDelete("{id:int}")] // api/autores/1
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
