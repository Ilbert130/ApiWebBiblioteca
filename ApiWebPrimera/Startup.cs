using ApiWebPrimera.Controllers;
using ApiWebPrimera.Filtros;
using ApiWebPrimera.Middlewares;
using ApiWebPrimera.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiWebPrimera
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Es para instanciar todas las clases que tengan dependencia de otra clase o una abstracion
        public void ConfigureServices(IServiceCollection services)
        {
            //Este servicio crea filtros globales
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));//Aqui estamos colocando el filtro creado como global
            }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //Esta es la configuracion del dbcontext para que cuando hagamos una dependencia de este, ya venga configurado con todas sus referencias
            //De manera automatica
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            //----------------------

            //Con esto estamos diciendo que cuando una clase requiera un IServicio, pasale ServicioA, Es decir, una instancia de la clase ServiceA 
            //services.AddTransient<IServices, ServicesA>();

            ////---------------------

            //services.AddTransient<ServicioTransient>();
            //services.AddScoped<ServicioScoped>();
            //services.AddSingleton<ServicioSingleton>();

            //services.AddTransient<MiFltroDeAccion>();
            //services.AddHostedService<EscribirEnArchivo>();

            //Asi se configura un tipo concreto, es decir, todos sus inyecciones
            //services.AddTransient<ServicesA>();


            //services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                    ClockSkew = TimeSpan.Zero
                });

            //-------------------------r

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            }
            );


            //Configurando el automappe en el sistma de inyecion de dependencia
            services.AddAutoMapper(typeof(Startup));

            //Asi configuramos los servicios del identity para trabajarlo
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Esta es la autorizacion basada en claim, Politicas de accesos (Seguridad)
            services.AddAuthorization(opcioens =>
            {
                opcioens.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
                //opcioens.AddPolicy("EsVendedor", politica => politica.RequireClaim("esVendedor"));
            });

            //Esto es relevante para aplicaciones que corren en el navegador
            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    //Aqui se colocan las URL que van a poder tener acceso a nuestra web api
                    //Con AllowAnyMethod se refiere a que podra tener acceso a Get, Post, Put, etc.
                    //Con AllowAnyHeader podra devolver las cabeseras
                    builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader();

                    //.WithExposedHeaders() es para exponer las cabezeras en una peticion.
                });
            });

            services.AddDataProtection();
        }

        //Los middlewar son el conjunto de tuberias que procesan una peticion http
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            ////app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            app.UseLoguearRespuestaHTTP();

            //app.Use(async (contexto, siguiente) =>
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        var cuerpoOriginalRespuesta = contexto.Response.Body;
            //        contexto.Response.Body = ms;

            //        await siguiente.Invoke();

            //ms.Seek(0, SeekOrigin.Begin);
            //string respuesta = new StreamReader(ms).ReadToEnd();
            //ms.Seek(0, SeekOrigin.Begin);

            //await ms.CopyToAsync(cuerpoOriginalRespuesta);
            //contexto.Response.Body = cuerpoOriginalRespuesta;

            //logger.LogInformation(respuesta);
            //    }
            //});



            //Este middlewar se ejecutara si se hace una peticion a esta rata
            //Route: server/ruta
            //app.Map("/ruta2", app =>
            //{
            //    //Intersepta la tuberia de proceso, ejecutando unicamente este bloque
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("Estoy iterceptando la tuberia");
            //    });

            //    app.Run(async (context) =>
            //    {
            //        await context.Response.WriteAsync("Hello World From 2nd Middleware");
            //    });
            //});


            //De lo contrario se ejecutaran estos
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            //app.UseResponseCaching();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
