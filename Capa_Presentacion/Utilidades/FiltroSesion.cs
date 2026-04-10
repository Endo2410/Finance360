using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Capa_Presentacion.Filtros
{
    public class FiltroSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verifica si hay IdUsuario en sesión
            var usuarioId = context.HttpContext.Session.GetInt32("IdUsuario");

            if (usuarioId == null)
            {
                // Si no hay sesión, redirige al login
                context.Result = new RedirectToActionResult("Index", "Acceso", null);
            }

            base.OnActionExecuting(context);
        }
    }
}

