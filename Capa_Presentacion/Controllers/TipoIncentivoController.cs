using Capa_Entidad;
using Capa_Negocio;
using Capa_Negocio.Incentivo;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class TipoIncentivoController : Controller
    {
        private readonly CN_TipoIncentivo cn = new();
        private readonly CN_Tipousoincentivo cnuso = new();
        private readonly CN_Estado cnEstado = new();

        public IActionResult Index()
        {
            return View(cn.Obtener());
        }

        // Listar Tipos de Publicidad
        public IActionResult Listar()
        {
            var lista = cn.Obtener();
            return Json(lista.Select(t => new { id = t.IdTipoIncentivo, nombre = t.Nombre }));
        }

        public IActionResult Estado()
        {
            var estados = cnEstado.ObtenerEstado("GENERAL", out string msg);

            if (!string.IsNullOrEmpty(msg))
                return Json(new { success = false, mensajes = new[] { msg } });

            return Json(new
            {
                success = true,
                estados = estados.Select(e => new { idEstado = e.IdEstado, nombre = e.Nombre })
            });
        }

        [HttpPost]
        public IActionResult Crear(TipoIncentivo obj)
        {
            bool ok = cn.Crear(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(TipoIncentivo obj)
        {
            bool ok = cn.Editar(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        // Listar Tipos de uso incentivo
        public IActionResult Tipouso()
        {
            return View(cnuso.ObtenerUso());
        }


        public IActionResult ListarUso()
        {
            var lista = cnuso.ObtenerUso();
            return Json(lista.Select(t => new { id = t.IdTipoUsoIncentivo, nombre = t.Nombre }));
        }

        [HttpPost]
        public IActionResult CrearUso(TipoUsoIncentivo obj)
        {
            bool ok = cnuso.CrearUso(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarUso(TipoUsoIncentivo obj)
        {
            bool ok = cnuso.EditarUso(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }
    }
}
