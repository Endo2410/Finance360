using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class NotaCreditoController : Controller
    {
        private readonly CN_NotaCredito objcn = new CN_NotaCredito();

        public IActionResult Index()
        {
            var lista = objcn.ListarNotasCredito();
            return View(lista);
        }


        private readonly string _rutaBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        public IActionResult Ver(string carpeta, string archivo)
        {
            if (string.IsNullOrEmpty(carpeta) || string.IsNullOrEmpty(archivo))
                return NotFound();

            string rutaArchivo = Path.Combine(_rutaBase, carpeta, archivo);

            if (!System.IO.File.Exists(rutaArchivo))
                return NotFound();

            string extension = Path.GetExtension(rutaArchivo).ToLower();
            string tipoContenido = extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return PhysicalFile(rutaArchivo, tipoContenido);
        }

        // Acción para devolver el detalle de pagos vía AJAX
        [HttpGet]
        public IActionResult VerDetallePago(int id)
        {
            List<DetallePagoNC> detalle = objcn.ObtenerDetallePago(id);
            return Json(detalle);  // retorna JSON para llenar el modal
        }
    }
}
