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
    [FiltroSesion]
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
        #endregion

        #region TIPO SERVICIOS BASICOS
        public IActionResult TipoServicio()
        {
            var lista = objServicio.ObtenerTipoServicio();
            return View(lista);
        }
        [HttpPost]
        public JsonResult GuardarTipoServicio(E_TipoServicio obj)
        {
            string mensaje = string.Empty;

            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                return Json(new { resultado = false, mensaje = "Sesión expirada" });
            }

            obj.IdUsuario = idUsuario.Value;



            bool respuesta = objServicio.Guardar(obj, out mensaje);

            return Json(new { resultado = respuesta, mensaje });
        }
        #endregion

        #region Clientes
        public IActionResult Clientes()
        {
            ViewBag.Estados=objEstado.ObtenerEstados();
            ViewBag.Sucursales=objCN.ObtenerSucursales();
            ViewBag.TipoServicios=objServicio.ObtenerTipoServicio();
            var lista = objCliente.ObtenerClientes();
            return View(lista);
        }
        [HttpPost]
        public JsonResult GuardarClientes(E_Clientes obj)
        {
            string mensaje = string.Empty;

            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                return Json(new { resultado = false, mensaje = "Sesión expirada" });
            }

            obj.IdUsuario = idUsuario.Value;



            bool respuesta = objCliente.Guardar(obj, out mensaje);

            return Json(new { resultado = respuesta, mensaje });
        }
        #endregion

        #region CUENTAS POR PAGAR
        public IActionResult CXP()
        {
    
            ViewBag.Clientes = objCliente.ObtenerClientes();
            ViewBag.Estados = objEstado.ObtenerEstados();
            ViewBag.Meses = objTC.Obtener();
            
            var lista = objCP.Listar();
            return View(lista);

            //return View(objCP.Listar());
        }

        [HttpPost]
        public IActionResult Guardar(
       [FromForm] E_CxP_Contabilidad_Alejandra obj,
       IFormFile fileReciboPendiente,
       IFormFile fileReciboPagado)
        {
            obj.IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            // 1️⃣ Guardar CXP primero
            bool respuesta = objCP.Guardar(obj, out string mensaje);

            if (!respuesta)
                return Json(new { resultado = false, mensaje });

            int idGenerado = obj.IdCxP; // Debe venir del SP

            // 2️⃣ Ruta física
            string rutaCarpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "CXP_CONTABILIDAD_ALEJANDRA"
            );

            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            // =============================
            // FUNCIÓN LOCAL PARA GUARDAR
            // =============================
            void GuardarArchivo(IFormFile archivo, string tipo)
            {
                if (archivo == null || archivo.Length == 0)
                    return;

                string extension = Path.GetExtension(archivo.FileName);
                string nombreSistema = $"{Guid.NewGuid()}{extension}";
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreSistema);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                objArchivoAdj.Guardar(new E_ArchivoAdjunto
                {
                    TablaReferencia = "CXP_CONTABILIDAD_ALEJANDRA",
                    IdReferencia = idGenerado,
                    NombreArchivo = archivo.FileName,
                    NombreSistema = nombreSistema,
                    Extension = extension,
                    RutaServidor = "/uploads/CXP_CONTABILIDAD_ALEJANDRA/" + nombreSistema,
                    TipoArchivo = tipo
                });

            }

            // 3️⃣ Guardar ambos archivos
            GuardarArchivo(fileReciboPendiente, "RECIBO_A_PAGAR");
            GuardarArchivo(fileReciboPagado, "RECIBO_PAGADO");

            return Json(new { resultado = true, mensaje });
        }




        #endregion



    }
}
