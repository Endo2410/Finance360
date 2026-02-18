using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
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

        // 🔹 CREAR
        [HttpPost]
        public IActionResult Crear(AlertaUsuario alerta)
        {
            bool exito = objcn.CrearAlertaUsuario(alerta, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Alerta asignada correctamente." };

            return Json(new { success = exito, mensajes });
        }

        // 🔹 EDITAR
        [HttpPost]
        public IActionResult Editar(AlertaUsuario alerta)
        {
            bool exito = objcn.EditarAlertaUsuario(alerta, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Alerta actualizada correctamente." };

            return Json(new { success = exito, mensajes });
        }
    }
}
