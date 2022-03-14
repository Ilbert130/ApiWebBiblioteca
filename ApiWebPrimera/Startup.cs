using ApiWebPrimera.Controllers;
using ApiWebPrimera.Filtros;
using ApiWebPrimera.Middlewares;
using ApiWebPrimera.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json.Serialization;

namespace ApiWebPrimera
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            //-------------------------r

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            //Configurando el automappe en el sistma de inyecion de dependencia
            services.AddAutoMapper(typeof(Startup));
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
                
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseResponseCaching();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
