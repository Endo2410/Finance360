using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class VencidosController : Controller
    {
        private readonly CN_Vencidos objcn = new();
        private readonly CN_PagoVencido objPago = new CN_PagoVencido();
        private readonly CN_Vencidos objVencidos = new();

        #region VENCIDO
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
        #endregion

        #region LISTA_VENCIDOS
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
        #endregion

        #region PAGOS_VENCIDOS
        public IActionResult Pagos_vencidos()
        {
            var lista = objVencidos.ObtenerVencidos() ?? new List<Vencido>();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);

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

                    // Asignar el usuario que registra el pago
                    detalleCuotas[i].usuarioPago = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
                }

                var retenciones = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<RetencionVM>>(form["Retenciones"]) ?? new List<RetencionVM>();

                //  Construir pago principal
                PagoVencido pago = new PagoVencido
                {
                    IdVencido = int.Parse(form["IdVencido"]),
                    FechaDocumento = DateTime.Now,
                    Observacion = form["Observacion"],
                    MontoTotal = totalAPagar,
                    DetalleCuotas = detalleCuotas,
                    Retenciones = retenciones
                };

                // Registrar en base
                bool exito = objPago.RegistrarPago(pago, out List<string> mensajes, out string numeroDocumento);

                return Json(new { success = exito, mensajes, numeroDocumento });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajes = new[] { "Error al registrar el pago: " + ex.Message } });
            }
        }
        #endregion
    }
}
