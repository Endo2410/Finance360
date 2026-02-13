using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class ModalidadController : Controller
    {
        private readonly CN_Modalidad objcn = new CN_Modalidad();
        private readonly CN_Estado objcn_Estado = new CN_Estado();

        public IActionResult Index()
        {
            List<Modalidad> lista = objcn.ObtenerModalidades();
            return View(lista);
        }

        // Listar Modalidades
        [HttpGet]
        public IActionResult Listar()
        {
            var lista = objcn.ObtenerModalidades();

            return Json(lista.Select(m => new
            {
                id = m.IdModalidad,
                nombre = m.Nombre,
                tipoIntervalo = m.TipoIntervalo,     // MES | DIA | ANIO
                valorIntervalo = m.ValorIntervalo    // 1 | 3 | 6 | 15
            }));
        }

        [HttpPost]
        public IActionResult Crear(Modalidad modalidad)
        {
            bool exito = objcn.CrearModalidad(modalidad, out List<string> mensajes);
            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(Modalidad modalidad)
        {
            bool exito = objcn.EditarModalidad(modalidad, out List<string> mensajes);
            return Json(new { success = exito, mensajes });
        }

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
