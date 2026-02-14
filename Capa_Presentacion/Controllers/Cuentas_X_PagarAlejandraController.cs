using Capa_Dato;
using Capa_Entidad;
using Capa_Negocio.Contabilidad_Alejandra;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    [FiltroSesion]
    public class Cuentas_X_PagarAlejandraController : Controller
    {
        private readonly CN_Sucursales objCN = new CN_Sucursales();
        public IActionResult Sucursales()
        {
            var lista = objCN.ObtenerSucursales();
            return View(lista);
        }

        [HttpPost]
        public JsonResult Sincronizar()
        {
            var resultado = objCN.SincronizarSucursales();

            return Json(new
            {
                insertados = resultado.insertados,
                actualizados = resultado.actualizados
            });
        }



    }
}
