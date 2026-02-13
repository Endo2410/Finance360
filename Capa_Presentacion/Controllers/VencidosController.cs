using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class VencidosController : Controller
    {
        private readonly CN_Vencidos objcn = new();

        public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue)
                fechaInicio = DateTime.Today.AddMonths(-1);

            if (!fechaFin.HasValue)
                fechaFin = DateTime.Today;

            var lista = objcn.ObtenerVencidos(fechaInicio.Value, fechaFin.Value);

            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");

            return View(lista);
        }

        [HttpPost]
        public IActionResult Sincronizar()
        {
            try
            {
                int filasInsertadas = objcn.InsertarVencidosNuevos();
                return Json(new { exito = true, insertadas = filasInsertadas });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
        }


        public IActionResult ListaVencidos(string doc = null)
        {
            var lista = objcn.ObtenerVencido1();

            // Pasar filtro desde Notas de Crédito si existe
            ViewBag.FiltroDocumento = doc;

            return View(lista);
        }

        [HttpGet]
        public IActionResult DetallePagoVencido(int id)
        {
            var detalles = objcn.ObtenerDetallePagoVencido(id);
            return Json(detalles);
        }

        [HttpPost]
        public IActionResult AnularPagoVencido(int idDetallePago)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                bool ok = objcn.AnularPagoVencido(idDetallePago, usuario);

                if (!ok)
                    return Json(new { success = false, mensaje = "No se pudo anular el pago." });

                return Json(new { success = true, mensaje = "Pago anulado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = ex.Message });
            }
        }
    }
}
