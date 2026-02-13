using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PagoRebateController : Controller
    {
        private readonly CN_EjecucionRebate objEjecucion = new();
        private readonly CN_PagoRebate objPago = new();
        public IActionResult Index()
        {
            var lista = objEjecucion.ObtenerEjecuciones();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPagoMultiples()
        {
            var form = Request.Form;

            var detalleEjecuciones = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<DetallePagoRebate>>(form["DetalleEjecuciones"]);

            if (!decimal.TryParse(form["TotalAPagar"], out decimal totalAPagar))
                return Json(new { success = false, mensajes = new[] { "Total a pagar inválido" } });

            if (detalleEjecuciones == null || !detalleEjecuciones.Any())
                return Json(new { success = false, mensajes = new[] { "Debe seleccionar al menos una ejecución" } });

            if (detalleEjecuciones.Any(d => d.MontoPagado <= 0))
                return Json(new { success = false, mensajes = new[] { "Los montos deben ser mayores a cero" } });

            // Guardar comprobantes
            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ComprobantesRebate");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            for (int i = 0; i < detalleEjecuciones.Count; i++)
            {
                var file = form.Files[$"PagosTipos[{i}].Comprobante"];
                if (file != null && file.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);
                    using var stream = new FileStream(ruta, FileMode.Create);
                    await file.CopyToAsync(stream);

                    detalleEjecuciones[i].RutaComprobante = nombreArchivo;
                }

                // ✅ Asignar el usuario que registra el pago
                detalleEjecuciones[i].usuarioPago = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            }

            PagoRebate pago = new()
            {
                IdAcuerdo = int.Parse(form["IdAcuerdo"]),
                FechaDocumento = DateTime.Now,
                Observacion = form["Observacion"],
                MontoTotal = totalAPagar,
                DetalleEjecuciones = detalleEjecuciones
            };

            bool exito = objPago.RegistrarPago(pago, out List<string> mensajes, out string numeroDocumento);
            return Json(new { success = exito, mensajes, numeroDocumento });
        }
    }
}
