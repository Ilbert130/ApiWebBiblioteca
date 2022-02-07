using ApiWebPrimera.Controllers.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiWebPrimera.Controllers
{
    [ApiController]
    [Route("api/bibleoteca")]
    public class BibliotecaControlle:ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Biblioteca>> Get()
        {
            List<Biblioteca> bibliotecas = new List<Biblioteca>();
            bibliotecas.AddRange(new Biblioteca[]
            {
                new Biblioteca() { Id = 1,Nombre="Ilbert"},
                new Biblioteca() { Id = 2,Nombre="Juan"},
                new Biblioteca() { Id = 3,Nombre="Pedro"},
                new Biblioteca() { Id = 4,Nombre="Martes"},
            });

            return bibliotecas;
        }


    }
}
