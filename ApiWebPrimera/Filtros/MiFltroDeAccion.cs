using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ApiWebPrimera.Filtros
{
    public class MiFltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFltroDeAccion> logger;

        public MiFltroDeAccion(ILogger<MiFltroDeAccion> logger)
        {
            this.logger = logger;
        }

        //Se ejecuta cuando la accion esta comenzando
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la accion");
        }

        //Se ejecuta cuando la accion ya termino
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de ejecutar la accion");
        }

        
    }
}
