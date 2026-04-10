using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class UsuariosController : Controller
    {
        private readonly CN_Usuario objcn = new CN_Usuario();
        private readonly CN_Estado objcn_Estado = new CN_Estado();

        public IActionResult Index()
        {
            List<Usuario> lista = objcn.ObtenerUsuarios(); // Método que trae todos los usuarios
            return View(lista);
        }

        [HttpPost]
        public IActionResult Crear(Usuario usuario)
        {
            bool exito = objcn.CrearUsuario(usuario, out List<string> mensajes);

            // Si no hay mensajes de éxito, añadimos uno por defecto
            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Usuario creado y correo enviado." };

            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            // Obtener ID del usuario logueado DESDE SESSION
            int idUsuarioLogueado = Convert.ToInt32(HttpContext.Session.GetInt32("IdUsuario"));

            // No puede desactivarse a sí mismo
            if (usuario.IdUsuario == idUsuarioLogueado && usuario.IdEstado == 0)
            {
                return Json(new
                {
                    success = false,
                    mensajes = new List<string> {
                        "No puede desactivar su propio usuario."
                    }
                });
            }

            usuario.Clave = null;
            bool exito = objcn.EditarUsuario(usuario, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Usuario actualizado correctamente." };

            return Json(new { success = exito, mensajes });
        }

       

        public IActionResult Permiso()
        {
            return View();
        }

        // Listar estados en JSON
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


        [HttpGet]
        public JsonResult UsuariosPorRol(int idRol)
        {
            var lista = objcn.UsuariosPorRol(idRol);
            return Json(lista);
        }
    }
}
