using Capa_Entidad;
using Capa_Negocio.Incentivo;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    public class IncentivoSaldoController : Controller
    {
        private readonly CN_IncentivoSaldo cn =
            new CN_IncentivoSaldo();

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ObtenerSaldo(int idSucursal)
        {
            var saldo =
                cn.ObtenerSaldo(idSucursal);

            return Json(saldo);
        }


        [HttpGet]
        public JsonResult ObtenerIncentivosRecibidos(int idSucursal)
        {
            var lista = cn.ObtenerIncentivosRecibidos(idSucursal);

            return Json(new { data = lista });
        }

        [HttpGet]
        public IActionResult ObtenerUsos(int idSucursal)
        {
            var lista = cn.ObtenerUsos(idSucursal);
            return Json(new { data = lista });
        }


        [HttpPost]
        public IActionResult GuardarUso()
        {
            try
            {
                var form = Request.Form;

                var archivo = form.Files["comprobante"];

                string nombreArchivo = "";

                if (archivo != null && archivo.Length > 0)
                {
                    string carpeta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/comprobantesmovimiento"
                    );

                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);

                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using var stream = new FileStream(rutaCompleta, FileMode.Create);
                    archivo.CopyTo(stream);
                }

                IncentivoMovimiento obj = new IncentivoMovimiento()
                {
                    IdSucursal = Convert.ToInt32(form["idsucursal"]),
                    IdTipoUso = Convert.ToInt32(form["idtipouso"]),
                    Monto = Convert.ToDecimal(form["monto"]),
                    Observacion = form["observacion"],

                    UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido",

                    Comprobante = nombreArchivo
                };

                bool respuesta = cn.Registrar(obj);

                return Json(new
                {
                    success = respuesta
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
