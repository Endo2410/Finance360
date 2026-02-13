using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PagoNCController : Controller
    {
        private readonly CN_NotaCredito objNotas = new CN_NotaCredito();
        private readonly CN_PagoNC objPago = new CN_PagoNC(); 

        // GET: Listado de notas pendientes
        public IActionResult Index()
        {
            var lista = objNotas.ListarNotasCredito() ?? new List<NotaCredito>();

            var notasEstado5 = lista
                .Where(x => x.IdEstado == 5)
                .ToList();

            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(notasEstado5); 
        }

        [HttpPost]
        public async Task<IActionResult> AplicarNotaCredito()
        {
            try
            {
                var form = Request.Form;

                // IDs de notas seleccionadas
                var notasIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(form["IdsNotas"]);
                if (notasIds == null || !notasIds.Any())
                    return Json(new { success = false, mensajes = new[] { "Debe seleccionar al menos una nota." } });

                // Deserializar detalle de cheques
                var detalleCheques = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<List<DetallePagoNC>>(form["DetalleAplicaciones"]);

                if (detalleCheques == null || !detalleCheques.Any())
                    return Json(new { success = false, mensajes = new[] { "Debe agregar al menos un cheque." } });

                // Guardar archivos
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ComprobantesNC");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                string usuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                for (int i = 0; i < detalleCheques.Count; i++)
                {
                    var file = form.Files[$"Aplicaciones[{i}].Comprobante"];
                    if (file != null && file.Length > 0)
                    {
                        string nombreArchivo = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        string ruta = Path.Combine(carpeta, nombreArchivo);
                        using var stream = new FileStream(ruta, FileMode.Create);
                        await file.CopyToAsync(stream);
                        detalleCheques[i].RutaComprobante = nombreArchivo;
                    }

                    detalleCheques[i].UsuarioPago = usuarioActual;
                }

                // Construir lista de notas
                List<NotaCredito> notas = notasIds.Select(id => new NotaCredito
                {
                    IdNC = id,
                    Observacion = form["Observacion"], 
                    DetallePagos = detalleCheques
                }).ToList();

                // Aplicar todas las notas usando CN_PagoNC
                bool exito = objPago.AplicarNotasCredito(notas, out List<string> mensajes, out List<string> numerosDocumentos);

                return Json(new { success = exito, mensajes, numerosDocumentos });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajes = new[] { "Error al aplicar las notas de crédito: " + ex.Message } });
            }
        }
    }
}

