using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    public class AccesoController : Controller
    {
        private readonly CN_Usuario cn_usuario = new CN_Usuario();

        // GET: /Acceso/Index
        [HttpGet]
        public IActionResult Index()
        {
            // Si ya está logueado, redirigir a Home
            if (HttpContext.Session.GetInt32("IdUsuario") != null)
                return RedirectToAction("Bienvenido", "Home");

            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        // POST: /Acceso/Index
        [HttpPost]
        public IActionResult Index(string nombreUsuario, string clave)
        {
            // Obtener usuario
            Usuario usuario = new CN_Usuario().ObtenerUsuarios()
                .Where(u => u.NombreUsuario == nombreUsuario
                         && u.Clave == CN_Recursos.ConvertirSha256(clave))
                .FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            // Usuario inactivo
            if (usuario.oEstado != null && usuario.oEstado.IdEstado == 2)
            {
                ViewBag.Error = "Usuario inactivo. Contacte al administrador.";
                return View();
            }

            // Obligar cambio de clave
            if (usuario.Reestablecer)
            {
                HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
                return RedirectToAction("CambiarClave");
            }

            // Guardar rol en sesión
            HttpContext.Session.SetString("RolUsuario", usuario.oRol.Nombre);

            // LOGIN NORMAL
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);

            // 👇 NUEVO
            if (usuario.IdSucursal != null)
            {
                HttpContext.Session.SetInt32("IdSucursal", usuario.IdSucursal.Value);
            }

            // Guardar nombre completo
            HttpContext.Session.SetString("NombreCompleto", $"{usuario.Nombres} {usuario.Apellidos}");

            // Guardar permisos
            var cn_permiso = new CN_Permiso();
            var permisos = cn_permiso.ObtenerEstructuraPorUsuario(usuario.IdUsuario);
            HttpContext.Session.SetString("MenuUsuario", System.Text.Json.JsonSerializer.Serialize(permisos));


            // acciones (solo acciones)
            var cn_permisos = new CN_Permiso();
            var acciones = cn_permisos.ObtenerAccionesPorUsuario(usuario.IdUsuario);


            HttpContext.Session.SetString(
                "AccionesUsuario",
                System.Text.Json.JsonSerializer.Serialize(acciones)
            );

            return RedirectToAction("Bienvenido", "Home");
        }



        // GET: /Acceso/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpiar toda la sesión
            return RedirectToAction("Index", "Acceso");
        }

        [HttpPost]
        public ActionResult CambiarClave(string nuevaclave, string confirmarclave)
        {
            int? idusuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idusuario == null)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(nuevaclave) || nuevaclave.Length < 4)
            {
                ViewBag.Error = "La nueva contraseña debe tener mínimo 4 caracteres";
                return View();
            }

            if (nuevaclave != confirmarclave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            string claveEncriptada = CN_Recursos.ConvertirSha256(nuevaclave);

            string mensaje;
            bool respuesta = new CN_Usuario()
                .CambiarClave(idusuario.Value, claveEncriptada, out mensaje);

            if (respuesta)
            {
                HttpContext.Session.Remove("IdUsuario");
                return RedirectToAction("Index");
            }

            ViewBag.Error = mensaje;
            return View();
        }


        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario ousuario = new Usuario();

            ousuario = new CN_Usuario().ObtenerUsuarios().Where(item => item.Correo == correo).FirstOrDefault();

            if (ousuario == null)
            {

                ViewBag.Error = "No se encontro un correo relacionado a ese correo ";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuario().RestablecerClave(ousuario.IdUsuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        public ActionResult CerrarSesion()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}

