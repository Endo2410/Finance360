using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class PermisoController : Controller
    {
        private readonly CN_Permiso objCN = new CN_Permiso();
        private readonly CN_Rol objRol = new CN_Rol();
        private readonly CN_Usuario objcn = new CN_Usuario();

        public IActionResult Index()
        {
            ViewBag.Roles = objRol.Obtener();
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerPermisos(int idRol)
        {
            var estructura = objCN.ObtenerEstructuraCompleta();

            // Ahora devuelve PermisosRolDTO
            PermisoRolDto permisosRol = idRol > 0 ? objCN.ObtenerPermisosPorRol(idRol) : new PermisoRolDto();

            var result = new
            {
                Estructura = estructura,
                PermisosRol = permisosRol
            };

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            return Json(result, jsonOptions);
        }

        public class PermisosRequest
        {
            public int IdRol { get; set; }
            public int IdUsuario { get; set; }
            public int[] Acciones { get; set; }
            public int[] SubMenus { get; set; }
            public int[] Modulos { get; set; }
        }


        [HttpPost]
        public JsonResult GuardarPermisos([FromBody] PermisosRequest request)
        {
            try
            {
                bool resultado = objCN.GuardarPermisos(request.IdRol, request.Acciones.ToList(), request.SubMenus.ToList(), request.Modulos.ToList());
                return Json(new { success = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarPermisosUsuario([FromBody] PermisosRequest request)
        {
            try
            {
                bool resultado = objCN.GuardarPermisosUsuario(
                    request.IdUsuario,
                    request.Acciones.ToList(),
                    request.SubMenus.ToList(),
                    request.Modulos.ToList()
                );
                return Json(new { success = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult UsuariosPorRol(int idRol)
        {
            var lista = objcn.UsuariosPorRol(idRol);
            return Json(lista);
        }


        [HttpGet]
        public JsonResult ObtenerPermisosUsuario(int idUsuario)
        {
            var permisosUsuario = objCN.ObtenerPermisosPorUsuario(idUsuario);

            return Json(permisosUsuario);
        }
    }
}

