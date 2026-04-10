using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Capa_Presentacion.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{

    [FiltroSesion]
    public class CanjeController : Controller
    {
        private readonly CN_Canje cn = new CN_Canje();

        public IActionResult Index()
        {
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(cn.ObtenerCanjes());
        }

        [HttpPost]
        public IActionResult Crear([FromForm] Canje canje, IFormFile archivo, IFormFile archivoActa)
        {
            string ruta = Path.Combine("wwwroot/archivo/canje");
            Directory.CreateDirectory(ruta);

            if (archivo != null)
            {
                string nombre = $"{Guid.NewGuid()}_{archivo.FileName}";
                using var fs = new FileStream(Path.Combine(ruta, nombre), FileMode.Create);
                archivo.CopyTo(fs);

                canje.DocumentoAdjunto = nombre;
            }

            // ARCHIVO ACTA
            if (archivoActa != null)
            {
                string nombreActa = $"{Guid.NewGuid()}_{archivoActa.FileName}";
                using var fs = new FileStream(Path.Combine(ruta, nombreActa), FileMode.Create);
                archivoActa.CopyTo(fs);

                canje.ArchivoActa = nombreActa;
            }

            canje.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            bool ok = cn.Crear(canje, out var mensajes);

            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Canje canje, IFormFile archivo, IFormFile archivoActa)
        {
            string ruta = Path.Combine("wwwroot/archivo/canje");
            Directory.CreateDirectory(ruta);

            if (archivo != null)
            {
                string nombre = $"{Guid.NewGuid()}_{archivo.FileName}";
                using var fs = new FileStream(Path.Combine(ruta, nombre), FileMode.Create);
                archivo.CopyTo(fs);

                canje.DocumentoAdjunto = nombre;
            }
            else
            {
                canje.DocumentoAdjunto = Request.Form["DOCUMENTO_EXISTENTE"];
            }

            // ACTA
            if (archivoActa != null)
            {
                string nombreActa = $"{Guid.NewGuid()}_{archivoActa.FileName}";
                using var fs = new FileStream(Path.Combine(ruta, nombreActa), FileMode.Create);
                archivoActa.CopyTo(fs);

                canje.ArchivoActa = nombreActa;
            }
            else
            {
                canje.ArchivoActa = Request.Form["ACTA_EXISTENTE"];
            }

            // AQUÍ PASAS EL USUARIO QUE MODIFICA
            canje.UsuarioModificacion = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            bool ok = cn.Editar(canje, out var mensajes);

            return Json(new { success = ok, mensajes });
        }

        public IActionResult ListaPagos(string doc = null)
        {
            var lista = cn.ObtenerCanjesresumen();

            // Pasar filtro desde Notas de Crédito si existe
            ViewBag.FiltroDocumento = doc;

            return View(lista);
        }


        [HttpGet]
        public IActionResult DetallePagoCanje(int id)
        {
            var detalles = cn.ObtenerDetallePagoCanje(id);
            return Json(detalles);
        }

        [HttpPost]
        public IActionResult AnularPagoCanje(int idDetallePago)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                bool resultado = cn.AnularPagoCanje(idDetallePago, usuario);

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
                    mensaje = "Pago de canje anulado correctamente."
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
