using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class EstadoCuentaController : Controller
    {
        private readonly CN_EstadoCuentaPublicidad objcn = new();

        public IActionResult Index()
        {
            var lista = objcn.ObtenerPendientes();
            return View(lista);
        }
    }
}
