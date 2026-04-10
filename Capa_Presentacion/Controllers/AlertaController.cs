using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class AlertaController : Controller
    {
        private readonly CN_AlertaUsuario objcn = new CN_AlertaUsuario();
        private readonly CN_Usuario objcnUsuario = new CN_Usuario();
        private readonly CN_Estado objcnEstado = new CN_Estado();

        public IActionResult Index()
        {
            List<AlertaUsuario> lista = objcn.ObtenerAlertaUsuario();
            return View(lista);
        }

        // 🔹 COMBO USUARIOS
        public IActionResult ListarUsuarios()
        {
            var lista = objcnUsuario.ObtenerUsuarios();

            var json = lista.Select(u => new
            {
                idUsuario = u.IdUsuario,
                nombre = u.Nombres + " " + u.Apellidos
            });

            return Json(json);
        }

        // 🔹 COMBO TIPO ALERTA
        public IActionResult ListarTipoAlerta()
        {
            var lista = objcn.ListarTipoAlerta();

            var json = lista.Select(t => new
            {
                idTipoAlerta = t.IdTipoAlerta,
                codigo = t.Codigo
            });

            return Json(json);
        }

        // 🔹 COMBO ESTADO
        public IActionResult Estado()
        {
            var listaEstados = objcnEstado.ObtenerEstado("GENERAL", out string mensaje);

            if (!string.IsNullOrEmpty(mensaje))
                return Json(new { success = false, mensajes = new List<string> { mensaje } });

            var jsonEstados = listaEstados.Select(e => new
            {
                idEstado = e.IdEstado,
                nombre = e.Nombre
            });

            return Json(new { success = true, estados = jsonEstados });
        }

        [HttpPost]
        public IActionResult GuardarAlertas([FromBody] AlertaUsuario alerta)
        {
            bool exito = objcn.GuardarAlertas(alerta, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Alertas guardadas correctamente." };

            return Json(new { success = exito, mensajes });
        }

       

        public IActionResult ObtenerPorUsuario(int idUsuario)
        {
            var lista = objcn.ObtenerAlertaUsuario()
                             .Where(x => x.IdUsuario == idUsuario)
                             .ToList();

            return Json(lista);
        }

        //nueva
        // 🔹 LISTAR ALERTAS (para dropdown)
        public IActionResult ObtenerAlertas()
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            var lista = objcn.ObtenerAlertasUsuario(idUsuario);

            return Json(lista);
        }

        // 🔹 CONTADOR (badge rojo)
        public IActionResult ContarAlertas()
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            int total = objcn.ContarAlertas(idUsuario);

            return Json(new { total });
        }

        // 🔹 MARCAR TODAS COMO VISTAS
        [HttpPost]
        public IActionResult MarcarVistas()
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            objcn.MarcarTodasComoVistas(idUsuario);

            return Json(new { ok = true });
        }
    }
}
