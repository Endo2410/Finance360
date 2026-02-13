using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class AcuerdoRebateController : Controller
    {
        private readonly CN_AcuerdoRebate objcn = new CN_AcuerdoRebate();

        public IActionResult Index()
        {
            var lista = objcn.ObtenerAcuerdos();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpGet]
        public IActionResult Listar()
        {
            try
            {
                var hoy = DateTime.Today;
                var lista = objcn.ObtenerAcuerdos()
                                 .Where(a => hoy >= a.FechaInicio && hoy <= a.FechaFin)
                                 .ToList();

                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Crear([FromForm] AcuerdoRebate acuerdo, IFormFile archivo)
        {
            // Carpeta donde se guardarán los archivos
            string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "acuerdos");
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

                // Guardar el nombre del archivo en el objeto
                acuerdo.Documento = nombreArchivo;
            }

            // Obtener el usuario logueado
            acuerdo.UsuarioCreacion = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            // Llamada al CN
            bool exito = objcn.CrearAcuerdo(acuerdo, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Acuerdo de rebate creado correctamente." };

            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar([FromForm] AcuerdoRebate acuerdo, IFormFile archivo)
        {
            string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "acuerdos");
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            if (archivo != null && archivo.Length > 0)
            {
                // Subir archivo nuevo
                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                acuerdo.Documento = nombreArchivo;
            }
            else
            {
                // Mantener archivo existente enviado desde el form
                acuerdo.Documento = Request.Form["DOCUMENTO_EXISTENTE"];
            }

            // Llamada al CN
            bool exito = objcn.EditarAcuerdo(acuerdo, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Acuerdo de rebate actualizado correctamente." };

            return Json(new { success = exito, mensajes });
        }
    }
}
