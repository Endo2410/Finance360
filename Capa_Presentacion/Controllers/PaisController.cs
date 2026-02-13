using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PaisController : Controller
    {
        private readonly CN_Pais objcn = new CN_Pais();
        private readonly CN_Estado objcn_Estado = new CN_Estado();

        public IActionResult Index()
        {
            List<Pais> lista = objcn.ObtenerPaises();
            return View(lista);
        }

        // Listar Países
        [HttpGet]
        public IActionResult ListarPaises()
        {           
            var lista = objcn.ObtenerPaises();
            return Json(lista.Select(p => new { id = p.IdPais, nombre = p.Nombre }));
        }

        [HttpPost]
        public IActionResult Crear(Pais pais)
        {
            bool exito = objcn.CrearPais(pais, out List<string> mensajes);
            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(Pais pais)
        {
            bool exito = objcn.EditarPais(pais, out List<string> mensajes);
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
