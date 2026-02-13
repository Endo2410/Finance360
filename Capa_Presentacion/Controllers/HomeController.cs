using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Capa_Presentacion.Models;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CapaPresentacion.Controllers
{
    [FiltroSesion]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CN_Dashboard cnDashboard = new CN_Dashboard();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin, int? idProveedor)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaFin < fechaInicio)
            {
                TempData["ErrorFecha"] = "La fecha fin no puede ser menor que la fecha inicio.";
                return RedirectToAction("Index");
            }

            var model = cnDashboard.ObtenerDashboard(fechaInicio, fechaFin, idProveedor);
            return View(model);
        }


        public IActionResult JefeArea()
        {
            // Cargar datos filtrados por departamento del jefe
            return View();
        }

        public IActionResult Administrador()
        {
            // Cargar datos filtrados por departamento del jefe
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
