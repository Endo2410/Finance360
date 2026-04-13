using Capa_Dato;
using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad;
using Capa_Entidad.CajaChica;
using Capa_Entidad.Contabilidad_Alejandra;
using Capa_Negocio;
using Capa_Negocio.CajaChica;
using Capa_Negocio.Contabilidad_Alejandra;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Capa_Presentacion.Controllers
{
    public class CajaChicaController : Controller
    {
        #region Caja Chica
        private readonly CN_CajaChica _cnCajaChica = new CN_CajaChica();
        private readonly CN_Usuario _cnUsuarios = new CN_Usuario();



        public IActionResult Movimientos()
        {
            // Definimos los IDs que SÍ pueden autorizar. 
            // Agregamos el 17 que es Yeries.
            int[] idsAutorizados = { 4,10,17 };

            List<Usuario> listaUsuarios = _cnUsuarios.ObtenerUsuarios()
                                             .Where(u => idsAutorizados.Contains(u.IdUsuario))
                                             .OrderBy(u => u.Nombres)
                                             .ToList();

            ViewBag.UsuariosAutorizadores = listaUsuarios;

            return View();
        }

        [HttpGet]
        public JsonResult ListarMovimientos()
        {
            List<Movimiento> lista = _cnCajaChica.Listar();
            return Json(new { data = lista });
        }

        [HttpPost]
        public JsonResult GuardarMovimiento([FromBody] Movimiento objeto)
        {
       
            string mensaje = string.Empty;

            // Recuperamos el ID del usuario logueado directamente de la sesión
            objeto.IdUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            if (objeto.IdUsuario == 0)
            {
                return Json(new { resultado = 0, mensaje = "Sesión expirada. Por favor inicie sesión nuevamente." });
            }

            int resultado = _cnCajaChica.RegistrarMovimiento(objeto, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje });
        }



        [HttpPost]
        public JsonResult AnularMovimiento(int id, string motivo)
        {
            string mensaje = string.Empty;

            // Recuperamos el ID del usuario que está anulando de la sesión
            int idUsuarioAnulador = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            if (idUsuarioAnulador == 0)
            {
                return Json(new { resultado = false, mensaje = "Sesión expirada. Inicie sesión nuevamente." });
            }

            // Pasamos el idUsuarioAnulador
            bool respuesta = _cnCajaChica.AnularMovimiento(id, motivo, idUsuarioAnulador, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult AplicarRetorno(int id, decimal monto, string motivo)
        {
            string mensaje = string.Empty;
            bool respuesta = _cnCajaChica.AplicarRetorno(id, monto, motivo, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje });
        }



        [HttpGet]
        public JsonResult ObtenerDetalleMovimiento(int id)
        {
            // Buscamos en la lista completa el movimiento que coincida con el ID
            // O puedes llamar a un método específico de tu Capa Negocio si existe
            Movimiento oMovimiento = _cnCajaChica.Listar().FirstOrDefault(m => m.IdMovimiento == id);

            if (oMovimiento != null)
            {
                return Json(new { success = true, data = oMovimiento });
            }
            else
            {
                return Json(new { success = false, mensaje = "No se encontró el movimiento." });
            }
        }


        public IActionResult ReportesMovimientos()
        {

            return View();
        }

        #endregion

        #region Cheques
        public IActionResult Cheques()
        {
            
            return View();
        }
        [HttpGet]
        public JsonResult ListarCheques()
        {
            List<Cheque> oLista = new CN_Cheques().Listar();
            return Json(new { data = oLista });
        }
        [HttpPost]
        public JsonResult GuardarCheque(string numero, string concepto, string entrada, IFormFile fotoCheque)
        {
            string mensaje = string.Empty;
            bool respuesta = false;

            try
            {
                // 1. Convertir IFormFile a byte[]
                byte[] imagenBytes = null;
                if (fotoCheque != null && fotoCheque.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        fotoCheque.CopyTo(ms);
                        imagenBytes = ms.ToArray();
                    }
                }

                // 2. Obtener el ID del usuario de la sesión de forma segura
                int idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

                if (idUsuarioSesion == 0)
                {
                    return Json(new { resultado = false, mensaje = "La sesión ha expirado, por favor inicie sesión nuevamente." });
                }

                // 3. Crear el objeto con conversiones seguras
                Cheque obj = new Cheque()
                {
                    NumeroCheque = Convert.ToInt32(numero),
                    Concepto = concepto,
                    // Usamos CultureInfo.InvariantCulture para evitar líos con puntos y comas decimales
                    Entrada = decimal.Parse(entrada, System.Globalization.CultureInfo.InvariantCulture),
                    IdUsuario = idUsuarioSesion, // <--- Corregido el error de sintaxis aquí
                    FechaRegistro = DateTime.Now,
                    Foto = imagenBytes
                };

                // 4. Llamar a la capa de negocio
                respuesta = new CN_Cheques().Registrar(obj, out mensaje);
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = "Ocurrió un error inesperado: " + ex.Message;
            }

            return Json(new { resultado = respuesta, mensaje = mensaje });
        }

        #endregion

    }
}
