using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class EjecucionRebateController : Controller
    {
        private readonly CN_EjecucionRebate objcn = new CN_EjecucionRebate();

        public IActionResult Index()
        {
            var lista = objcn.ObtenerEjecuciones();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpPost]
        public JsonResult Guardar(EjecucionRebate obj, IFormFile fileSoporte)
        {
            string mensaje;
            string nombreArchivo = null;

            try
            {
                // Guardar archivo físicamente si existe
                if (fileSoporte != null && fileSoporte.Length > 0)
                {
                    string carpeta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "archivo",
                        "rebate"
                    );

                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    // Nombre único con GUID + extensión original
                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileSoporte.FileName);
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        fileSoporte.CopyTo(stream);
                    }
                }

                //  Asignar archivo y usuario 
                obj.ArchivoSoporte = nombreArchivo;
                obj.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                // 3️⃣ Guardar ejecución
                bool resultado = objcn.CrearEjecucion(obj, out mensaje);

                return Json(new { resultado, mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Editar(EjecucionRebate obj, IFormFile fileSoporte)
        {
            string mensaje;
            string nombreArchivo = obj.ArchivoSoporte; // Mantener archivo anterior si no se envía uno nuevo

            try
            {
                // Guardar archivo nuevo si existe
                if (fileSoporte != null && fileSoporte.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "rebate");

                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileSoporte.FileName);
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        fileSoporte.CopyTo(stream);
                    }
                }

                obj.ArchivoSoporte = nombreArchivo;

                // Llamar SP para editar ejecución
                bool resultado = objcn.EditarEjecucion(obj, out mensaje);

                return Json(new { resultado, mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }

        public IActionResult ListaPagosRebate(string doc = null)
        {
            var lista = objcn.ObtenerEjecucionesRebateResumen();

            // Pasar filtro desde Notas de Crédito si existe
            ViewBag.FiltroDocumento = doc;

            return View(lista);

        }

        [HttpGet]
        public IActionResult DetallePagoEjecucion(int id)
        {
            try
            {
                var detalles = objcn.ObtenerDetallePagoEjecucionRebate(id);
                return Json(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AnularPagoEjecucionRebate(int idDetallePago)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                bool resultado = objcn.AnularPagoEjecucionRebate(idDetallePago, usuario);

                if (!resultado)
                {
                    return Json(new
                    {
                        success = false,
                        mensaje = "No se pudo anular el pago."
                    });
                }

                return Json(new
                {
                    success = true,
                    mensaje = "Pago anulado correctamente."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    mensaje = ex.Message
                });
            }
        }
    }
}
