using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PagoVencidoController : Controller
    {
        private readonly CN_PagoVencido objPago = new CN_PagoVencido();
        private readonly CN_Vencidos objVencidos = new();

        public IActionResult Index()
        {
            var lista = objVencidos.ObtenerVencidos() ?? new List<Vencido>();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista); // Modelo principal: lista de vencidos

        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPagoMultiples()
        {
            try
            {
                var form = Request.Form;

                // 🔹 Deserializar detalles de pagos
                var detalleCuotas = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<List<DetallePagoVencido>>(form["DetallePagos"]);

                if (!decimal.TryParse(form["TotalAPagar"], out decimal totalAPagar))
                    return Json(new { success = false, mensajes = new[] { "Total a pagar inválido." } });

                if (detalleCuotas == null || !detalleCuotas.Any())
                    return Json(new { success = false, mensajes = new[] { "Debe agregar al menos un pago." } });

                if (detalleCuotas.Any(d => d.MontoPagado <= 0))
                    return Json(new { success = false, mensajes = new[] { "Los montos deben ser mayores a cero." } });

                // 🔹 Guardar archivos
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ComprobantesVencidos");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                for (int i = 0; i < detalleCuotas.Count; i++)
                {
                    var file = form.Files[$"PagosTipos[{i}].Comprobante"];
                    if (file != null && file.Length > 0)
                    {
                        string nombreArchivo = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        string ruta = Path.Combine(carpeta, nombreArchivo);
                        using (var stream = new FileStream(ruta, FileMode.Create))
                            await file.CopyToAsync(stream);

                        detalleCuotas[i].RutaComprobante = nombreArchivo;
                    }

                    // ✅ Asignar el usuario que registra el pago
                    detalleCuotas[i].usuarioPago = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
                }

                // 🔹 Construir pago principal
                PagoVencido pago = new PagoVencido
                {
                    IdVencido = int.Parse(form["IdVencido"]),
                    FechaDocumento = DateTime.Now,
                    Observacion = form["Observacion"],
                    MontoTotal = totalAPagar,
                    DetalleCuotas = detalleCuotas
                };

                // 🔹 Registrar en base
                bool exito = objPago.RegistrarPago(pago, out List<string> mensajes, out string numeroDocumento);

                return Json(new { success = exito, mensajes, numeroDocumento });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajes = new[] { "Error al registrar el pago: " + ex.Message } });
            }
        }
    }
}
   