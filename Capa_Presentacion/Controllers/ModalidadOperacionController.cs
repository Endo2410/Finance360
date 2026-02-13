using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class ModalidadOperacionController : Controller
    {
        private readonly CN_ModalidadOperacion cn = new();
        private readonly CN_Estado cnEstado = new();

        public IActionResult Index()
        {
            return View(cn.ObtenerModalidades());
        }

        public IActionResult Listar()
        {
            var lista = cn.ObtenerModalidades();
            return Json(lista.Select(t => new { id = t.IdModalidadOp, nombre = t.Nombre }));
        }

        [HttpPost]
        public IActionResult Crear(ModalidadOperacion obj)
        {
            bool ok = cn.Crear(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(ModalidadOperacion obj)
        {
            bool ok = cn.Editar(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }

        public IActionResult Estado()
        {
            var lista = cnEstado.ObtenerEstado("GENERAL", out string msg);

            if (!string.IsNullOrEmpty(msg))
                return Json(new { success = false, mensajes = new List<string> { msg } });

            return Json(new
            {
                success = true,
                estados = lista.Select(e => new { idEstado = e.IdEstado, nombre = e.Nombre })
            });
        }
    }
}
