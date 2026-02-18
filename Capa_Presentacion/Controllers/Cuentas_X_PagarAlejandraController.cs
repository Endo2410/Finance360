using Capa_Dato;
using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad;
using Capa_Entidad.Contabilidad_Alejandra;
using Capa_Negocio;
using Capa_Negocio.Contabilidad_Alejandra;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class Cuentas_X_PagarAlejandraController : Controller
    {
        private readonly CN_Sucursales objCN = new CN_Sucursales();
        private readonly CN_TipoServicio objServicio = new CN_TipoServicio();

        private readonly CN_Clientes objCliente = new CN_Clientes();

        private readonly CN_Estado objEstado = new CN_Estado();

        private CN_CxP_Contabilidad_Alejandra objCP = new();

        private CN_TipoCanje objTC = new();
        private CN_ArchivoAdjunto objArchivoAdj = new();


        #region SUCURSALES

        public IActionResult Sucursales()
        {
            var lista = objCN.ObtenerSucursales();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var lista = objCN.ObtenerSucursales();
            return Json(lista.Select(p => new {
                idsucursal = p.IdSucursal,
                nombre = p.NombreSucursal,
                codigo = p.Codigo
            }));
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
