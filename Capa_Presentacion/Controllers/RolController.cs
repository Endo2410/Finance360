using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{

    [FiltroSesion]
    public class RolController : Controller
    {
        private readonly CN_Rol cn = new();
        private readonly CN_Estado cnEstado = new();

        public IActionResult Index()
        {
            return View(cn.Obtener());
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var roles = cn.Obtener().Select(r => new
            {
                idRol = r.IdRol,
                nombre = r.Nombre
            });

            return Json(roles);
        }

        [HttpPost]
        public IActionResult Crear(Rol rol)
        {
            bool ok = cn.Crear(rol, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(Rol rol)
        {
            bool ok = cn.Editar(rol, out var mensajes);
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
