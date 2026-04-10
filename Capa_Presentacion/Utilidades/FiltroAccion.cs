using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Utilidades
{
    public class FiltroAccion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"]?.ToString();

            if (string.IsNullOrEmpty(action) || action == "Index")
            {
                context.Result = new RedirectToActionResult("LostInSpace", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
