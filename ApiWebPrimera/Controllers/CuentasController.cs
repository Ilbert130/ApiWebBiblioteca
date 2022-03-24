using ApiWebPrimera.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiWebPrimera.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController:ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuretion;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDataProtector dataProtector;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuretion, SignInManager<IdentityUser> signInManager
            ,IDataProtectionProvider dataProtectionProvider)
        {
            this.userManager = userManager;
            this.configuretion = configuretion;
            this.signInManager = signInManager;
            //Aqui colocamos lo que se conoce como string de proposito
            dataProtector = dataProtectionProvider.CreateProtector("Valor_unico_Y_QUIZAS_SECRETO");
        }

        //Incriptacion que se puede desencriptar
        [HttpGet("encriptar")]
        public ActionResult Encriptar()
        {
            var textoPlano = "Ilbert Castillo";
            var txtoCifrado = dataProtector.Protect(textoPlano);
            var textoDecifrado = dataProtector.Unprotect(txtoCifrado);

            return Ok(new
            {
                textoPlano = textoPlano,
                txtoCifrado = txtoCifrado,
                textoDecifrado = textoDecifrado
            });
        }

        //Incriptacion por tiempo. Si pasa el tiempo establecido no se puede desencriptar.
        [HttpGet("encriptarPorTiempo")]
        public ActionResult EncriptarPorTiempo()
        {
            var protectorLimitadoPorTiempo = dataProtector.ToTimeLimitedDataProtector();
            var textoPlano = "Ilbert Castillo";
            var txtoCifrado = protectorLimitadoPorTiempo.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            var textoDecifrado = dataProtector.Unprotect(txtoCifrado);

            return Ok(new
            {
                textoPlano = textoPlano,
                txtoCifrado = txtoCifrado,
                textoDecifrado = textoDecifrado
            });
        }

        [HttpPost("registrar")] //api/cuentas/registrar
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuarios  credencialesUsuarios)
        {
            var usario = new IdentityUser { UserName = credencialesUsuarios.Email, Email = credencialesUsuarios.Email };
            var resultado = await userManager.CreateAsync(usario,credencialesUsuarios.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
         
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuarios credencialesUsuarios)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuarios.Email, credencialesUsuarios.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpGet("renovartoken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesUsuario = new CredencialesUsuarios()
            {
                Email = email
            };

            return await ConstruirToken(credencialesUsuario);
        }

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoveAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }


        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuarios credencialesUsuarios)
        {
            var claim = new List<Claim>()
            {
                new Claim("email",credencialesUsuarios.Email),
                new Claim("lo que yo quiera","cualquier otro valor")
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuarios.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claim.AddRange(claimsDB);

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
