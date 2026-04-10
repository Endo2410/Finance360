using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PublicidadController : Controller
    {
        private readonly CN_CampaniaPublicitaria objcn = new CN_CampaniaPublicitaria();

        public IActionResult Index()
        {
            var lista = objcn.ObtenerCampanias();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpPost]
        public IActionResult Crear([FromForm] CampaniaPublicitaria campania, IFormFile archivo)
        {
            string mensaje = string.Empty;

            // Carpeta donde se guardarán los archivos
            string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "campanias");
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            if (archivo != null && archivo.Length > 0)
            {
                // Generar un nombre único para el archivo
                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                // Guardar solo el nombre del archivo o la ruta relativa en el objeto
                campania.DocumentoAdjunto = nombreArchivo; // o "/uploads/campanias/" + nombreArchivo
            }

            // Obtener el usuario logueado
            campania.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            // Llamar al SP a través de CN
            bool exito = objcn.CrearCampania(campania, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Campaña creada correctamente." };

            return Json(new { success = exito, mensajes });
        }


        [HttpPost]
        public IActionResult Editar([FromForm] CampaniaPublicitaria campania, IFormFile archivo)
        {
            // Carpeta donde se guardarán los archivos si se reemplaza
            string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "campanias");
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            if (archivo != null && archivo.Length > 0)
            {
                // Archivo nuevo
                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                campania.DocumentoAdjunto = nombreArchivo;
            }
            else
            {
                // Usar archivo existente enviado desde el form
                campania.DocumentoAdjunto = Request.Form["DocumentoAdjunto"];
            }

            campania.UsuarioModificacion = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            // Llamada al CN
            bool exito = objcn.EditarCampania(campania, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Campaña actualizada correctamente." };

            return Json(new { success = exito, mensajes });
        }

        public IActionResult ListaPagos(string doc = null)
        {
            var lista = objcn.ObtenerCampaniasResumen();


            ViewBag.FiltroDocumento = doc; // si viene doc, la vista lo usará para filtrar
            return View(lista);
        }



        [HttpGet]
        public IActionResult DetallePagoCampania(int id)
        {
            var detalles = objcn.ObtenerDetallePagoCampania(id);
            return Json(detalles);
        }


        [HttpPost]
        public IActionResult AnularPagoPublicidad(int idDetallePago)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                bool resultado = objcn.AnularPagoPublicidad(idDetallePago, usuario);

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
