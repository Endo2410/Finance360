using Capa_Entidad;
using Capa_Entidad.Cuentas_Por_Pagar;
using Capa_Negocio;
using Capa_Negocio.Cuentas_Por_Pagar;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class TipoRetencionController : Controller
    {
        private readonly CN_TipoRetencion cn = new();
        private readonly CN_Estado cnEstado = new();

        public IActionResult Index()
        {
            return View(cn.Obtener());
        }

        public IActionResult Listar()
        {
            var lista = cn.Obtener();
            return Json(lista.Select(t => new { id = t.IdTipoRetencion, nombre = t.Nombre, porcentaje = t.Porcentaje }));
        }

        [HttpPost]
        public IActionResult Crear(TipoRetencion obj)
        {
            bool ok = cn.Crear(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(TipoRetencion obj)
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


        private readonly CN_TipoDescuentoPP cndescuento = new();

        public IActionResult Descuento()
        {
            return View(cndescuento.Obtener());
        }

        public IActionResult ListarDescuento()
        {
            var lista = cndescuento.Obtener();

            return Json(lista.Select(t => new
            {
                id = t.IdTipoDescuento,
                nombre = t.Nombre,
                porcentaje = t.Porcentaje
            }));
        }

        [HttpPost]
        public IActionResult CrearDescuento(TipoDescuentoPP obj)
        {
            bool ok = cndescuento.Crear(obj, out List<string> mensajes);

            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarDescuento(TipoDescuentoPP obj)
        {
            bool ok = cndescuento.Editar(obj, out List<string> mensajes);

            return Json(new { success = ok, mensajes });
        }
      
    }
}
