using ApiWebPrimera.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiWebPrimera.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController:ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuretion;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuretion)
        {
            this.userManager = userManager;
            this.configuretion = configuretion;
        }

        [HttpPost("registrar")] //api/cuentas/registrar
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuarios  credencialesUsuarios)
        {
            var usario = new IdentityUser { UserName = credencialesUsuarios.Email, Email = credencialesUsuarios.Email };
            var resultado = await userManager.CreateAsync(usario,credencialesUsuarios.Password);

            if (resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
         
        }

        private RespuestaAutenticacion ConstruirToken(CredencialesUsuarios credencialesUsuarios)
        {
            var claim = new List<Claim>()
            {
                new Claim("email",credencialesUsuarios.Email),
                new Claim("lo que yo quiera","cualquier otro valor")
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuretion["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claim, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }
    }
}
