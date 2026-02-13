using Capa_Entidad;
using Capa_Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    public class EstadoController : Controller
    {
        private readonly CN_Estado objCN = new CN_Estado();

        public IActionResult Index()
        {
            var lista = objCN.ObtenerEstados();
            return View(lista);
        }

        [HttpPost]
        public IActionResult Crear(Estado obj)
        {
            try
            {
                bool exito = objCN.CrearEstado(obj, out List<string> mensajes);
                if (exito && mensajes.Count == 0) mensajes.Add("Estado creado correctamente.");
                return Json(new { success = exito, mensajes });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, mensajes = new List<string> { ex.Message } });
            }
        }

        [HttpPost]
        public IActionResult Editar(Estado obj)
        {
            try
            {
                bool exito = objCN.EditarEstado(obj, out List<string> mensajes);
                if (exito && mensajes.Count == 0) mensajes.Add("Estado actualizado correctamente.");
                return Json(new { success = exito, mensajes });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, mensajes = new List<string> { ex.Message } });
            }
        }
    }
}
