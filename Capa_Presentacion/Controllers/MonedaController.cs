using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class MonedaController : Controller
    {
        private readonly CN_Moneda objcn = new CN_Moneda();
        private readonly CN_Estado objcn_Estado = new CN_Estado();

        public IActionResult Index()
        {
            List<Moneda> lista = objcn.ObtenerMonedas();
            return View(lista);
        }

        // Listar Monedas
        [HttpGet]
        public IActionResult ListarMonedas()
        {
            var lista = objcn.ObtenerMonedas();
            return Json(lista.Select(m => new { id = m.IdMoneda, nombre = m.Nombre, simbolo = m.Simbolo }));
        }


        [HttpPost]
        public IActionResult Crear(Moneda moneda)
        {
            bool exito = objcn.CrearMoneda(moneda, out List<string> mensajes);
            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(Moneda moneda)
        {
            bool exito = objcn.EditarMoneda(moneda, out List<string> mensajes);
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
