using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PagoCanjeController : Controller
    {
        private readonly CD_Canje objEjecucion = new();
        private readonly CN_PagoCanje objPago = new();

        public IActionResult Index()
        {
        
            var lista = objEjecucion.ObtenerCanjes();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);  
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPagoMultiples()
        {
            var form = Request.Form;

            var detalleCanjes = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<DetallePagoCanje>>(form["DetalleCanjes"]);

            if (!decimal.TryParse(form["TotalAPagar"], out decimal totalAPagar))
                return Json(new { success = false, mensajes = new[] { "Total a pagar inválido" } });

            if (detalleCanjes == null || !detalleCanjes.Any())
                return Json(new { success = false, mensajes = new[] { "Debe seleccionar al menos un canje" } });

            if (detalleCanjes.Any(d => d.MontoPagado <= 0))
                return Json(new { success = false, mensajes = new[] { "Los montos deben ser mayores a cero" } });


            // Guardar comprobantes
            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ComprobantesCanje");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            for (int i = 0; i < detalleCanjes.Count; i++)
            {
                var file = form.Files[$"PagosTipos[{i}].Comprobante"];
                if (file != null && file.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);
                    using var stream = new FileStream(ruta, FileMode.Create);
                    await file.CopyToAsync(stream);

                    detalleCanjes[i].RutaComprobante = nombreArchivo;
                }

                detalleCanjes[i].UsuarioPago = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            }

                 var retenciones = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<RetencionVM>>(form["Retenciones"]) ?? new List<RetencionVM>();

            PagoCanje pago = new()
            {
                IdCanje = int.Parse(form["IdCanje"]),
                MontoTotal = totalAPagar,
                Observacion = form["Observacion"],
                DetalleCanjes = detalleCanjes,
                Retenciones = retenciones
            };

            bool exito = objPago.RegistrarPago(pago, out List<string> mensajes, out string numeroDocumento);
            return Json(new { success = exito, mensajes, numeroDocumento });
        }
    }
}
