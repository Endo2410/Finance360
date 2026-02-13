using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    public class AlertaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
