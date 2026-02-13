using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PagoPublicidadController : Controller
    {
        private readonly CN_EstadoCuentaPublicidad objEstado = new();
        private readonly CN_PagoPublicidad objPago = new();

        public IActionResult Index()
        {
            var lista = objEstado.ObtenerPendientes();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPagoMultiples()
        {
            var form = Request.Form;

            var detalleCuotas = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<DetallePagoPublicidad>>(form["DetalleCuotas"]);

            if (!decimal.TryParse(form["TotalAPagar"], out decimal totalAPagar))
                return Json(new { success = false, mensajes = new[] { "Total a pagar inválido" } });

            if (detalleCuotas == null || !detalleCuotas.Any())
                return Json(new { success = false, mensajes = new[] { "Debe seleccionar al menos una cuota" } });

            if (detalleCuotas.Any(d => d.MontoPagado <= 0))
                return Json(new { success = false, mensajes = new[] { "Los montos deben ser mayores a cero" } });


            // Guardar comprobantes
            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Comprobantes");
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

            PagoPublicidad pago = new PagoPublicidad
            {
                IdCampania = int.Parse(form["IdCampania"]),
                FechaDocumento = DateTime.Now,
                Observacion = form["Observacion"],
                MontoTotal = totalAPagar,
                DetalleCuotas = detalleCuotas
            };

            bool exito = objPago.RegistrarPago(pago, out List<string> mensajes, out string numeroDocumento);
            return Json(new { success = exito, mensajes, numeroDocumento });
        }


        
        // Clase auxiliar
        public class PagoTipoVM
        {
            public int IdTipoDocumento { get; set; }
            public decimal Monto { get; set; }
            public string RutaComprobante { get; set; }
        }
    }
}
