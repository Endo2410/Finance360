using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using Humanizer;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
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


        public IActionResult Prueba(DateTime? inicio, DateTime? fin, string proveedor)
        {
            if (!inicio.HasValue)
                inicio = new DateTime(DateTime.Today.Year, 1, 1); // inicio del año

            if (!fin.HasValue)
                fin = DateTime.Today;

            var lista = objcn.ObtenerOrdenes(inicio.Value, fin.Value)
                             .Where(x => x.IdEstado == 5)   // SOLO PENDIENTES
                             .ToList();

            if (!string.IsNullOrEmpty(proveedor))
                lista = lista.Where(x => x.Proveedor == proveedor).ToList();

            return View(lista);
        }


    }
}
