using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class TipoRebateController : Controller
    {
        private readonly CN_TipoRebate cn = new();
        private readonly CN_Estado cnEstado = new();

        public IActionResult Index()
        {
            return View(cn.Obtener());
        }

        // Listar Tipos de rebate
        public IActionResult Listar()
        {
            var lista = cn.Obtener();
            return Json(lista.Select(t => new { id = t.IdTipoRebate, nombre = t.Nombre }));
        }

        [HttpPost]
        public IActionResult Crear(TipoRebate obj)
        {
            bool ok = cn.Crear(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(TipoRebate obj)
        {
            bool ok = cn.Editar(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        public IActionResult Estado()
        {
            var estados = cnEstado.ObtenerEstado("GENERAL", out string msg);

            if (!string.IsNullOrEmpty(msg))
                return Json(new { success = false, mensajes = new[] { msg } });

            return Json(new
            {
                success = true,
                estados = estados.Select(e => new
                {
                    idEstado = e.IdEstado,
                    nombre = e.Nombre
                })
            });
        }
    }
}
