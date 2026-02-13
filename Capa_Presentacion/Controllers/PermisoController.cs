using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class PermisoController : Controller
    {
        private readonly CN_Permiso objCN = new CN_Permiso();
        private readonly CN_Rol objRol = new CN_Rol();

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
    }
}
