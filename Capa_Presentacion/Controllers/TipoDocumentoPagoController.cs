using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class TipoDocumentoPagoController : Controller
    {
        private readonly CN_TipoDocumentoPago objcn = new CN_TipoDocumentoPago();
        private readonly CN_Estado objcn_Estado = new CN_Estado();

        public IActionResult Index()
        {
            List<TipoDocumentoPago> lista = objcn.ObtenerTipos();
            return View(lista);
        }

        // Listar Tipos de Documento
        [HttpGet]
        public IActionResult ListarTipos()
        {
            var lista = objcn.ObtenerTipos();
            return Json(lista.Select(m => new { id = m.IdTipoDoc, nombre = m.Nombre }));
        }

        [HttpPost]
        public IActionResult Crear(TipoDocumentoPago tipo)
        {
            bool exito = objcn.CrearTipo(tipo, out List<string> mensajes);
            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(TipoDocumentoPago tipo)
        {
            bool exito = objcn.EditarTipo(tipo, out List<string> mensajes);
            return Json(new { success = exito, mensajes });
        }

        // Listar estados solo del módulo GENERAL
        public IActionResult Estado()
        {
            var listaEstados = objcn_Estado.ObtenerEstado("GENERAL", out string mensaje);

            if (!string.IsNullOrEmpty(mensaje))
                return Json(new { success = false, mensajes = new List<string> { mensaje } });

            var jsonEstados = listaEstados.Select(e => new
            {
                idEstado = e.IdEstado,
                nombre = e.Nombre
            });

            return Json(new { success = true, estados = jsonEstados });
        }
    }
}
