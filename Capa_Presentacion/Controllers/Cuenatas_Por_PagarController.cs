using Capa_Entidad.Cuentas_Por_Pagar;
using Capa_Negocio;
using Capa_Negocio.Cuentas_Por_Pagar;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    public class Cuentas_Por_PagarController : Controller
    {
        private CN_CondicionPagoProveedor cn = new();
        private readonly CN_OrdenCompra objcn = new();


        public IActionResult CondicionPago()
        {
            return View(cn.Obtener());
        }

        [HttpPost]
        public IActionResult Crear(CondicionPagoProveedor obj)
        {
            bool ok = cn.Crear(obj, out var mensajes);

            return Json(new { success = ok, mensajes });
        }


        [HttpPost]
        public IActionResult Editar(CondicionPagoProveedor obj)
        {
            bool ok = cn.Editar(obj, out var mensajes);

            return Json(new
            {
                success = ok,
                mensajes
            });
        }

        public IActionResult PrepararFacturar(DateTime? inicio, DateTime? fin, string proveedor)
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
