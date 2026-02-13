using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class OrdenCompraController : Controller
    {
        private readonly CN_OrdenCompra objcn = new();

        public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue)
                fechaInicio = DateTime.Today.AddMonths(-1);

            if (!fechaFin.HasValue)
                fechaFin = DateTime.Today;

            var lista = objcn.ObtenerOrdenes(fechaInicio.Value, fechaFin.Value);

            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");

            return View(lista);
        }

        [HttpPost]
        public IActionResult Sincronizar()
        {
            try
            {
                int filasInsertadas = objcn.InsertarOrdenesNuevas();
                return Json(new { exito = true, insertadas = filasInsertadas });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
        }
        public IActionResult Reporte(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue) fechaInicio = DateTime.Today.AddMonths(-1);
            if (!fechaFin.HasValue) fechaFin = DateTime.Today;

            var lista = objcn.ObtenerReporteOrdenes(fechaInicio.Value, fechaFin.Value);

            // ORDEN CORRECTO:
            // 1. FechaCreacion DESC (últimos días primero)
            // 2. NumeroOrden DESC (mayor a menor)
            lista = lista
                .OrderByDescending(o => o.FechaCreacion)
                .ThenByDescending(o =>
                {
                    if (int.TryParse(o.NumeroOrden, out int num))
                        return num;

                    return 0; // Si no es numérico, lo manda abajo
                })
                .ToList();

            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");

            return View(lista);
        }

        private string[] ObtenerColumnas(OrdenCompra o)
        {
            return new string[]
            {
                o.Proveedor,
                o.NumeroOrden,
                o.Estado,
                o.Confirmacion,
                o.Observaciones,
                o.FechaCreacion.ToString("dd/MM/yyyy")
            };
        }
    }
}
