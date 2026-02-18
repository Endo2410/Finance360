using Capa_Entidad;
using Capa_Negocio;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class IncentivoController : Controller
    {
        private readonly CN_Incentivo cn = new();

        public IActionResult Index()
        {
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(cn.Obtener());
        }

        [HttpPost]
        public IActionResult Crear([FromForm] Incentivo incentivo, IFormFile archivo)
        {
            if (archivo != null)
            {
                string ruta = Path.Combine("wwwroot/archivo/incentivo");
                Directory.CreateDirectory(ruta);

                string nombre = $"{Guid.NewGuid()}_{archivo.FileName}";
                using var fs = new FileStream(Path.Combine(ruta, nombre), FileMode.Create);
                archivo.CopyTo(fs);

                incentivo.DocumentoAdjunto = nombre;
            }

            incentivo.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            bool ok = cn.Crear(incentivo, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Incentivo incentivo, IFormFile archivo)
        {
            if (archivo != null)
            {
                string ruta = Path.Combine("wwwroot/archivo/incentivo");
                Directory.CreateDirectory(ruta);

                string nombre = $"{Guid.NewGuid()}_{archivo.FileName}";
                using var fs = new FileStream(Path.Combine(ruta, nombre), FileMode.Create);
                archivo.CopyTo(fs);

                incentivo.DocumentoAdjunto = nombre;
            }
            else
            {
                incentivo.DocumentoAdjunto = Request.Form["DOCUMENTO_EXISTENTE"];
            }

            bool ok = cn.Editar(incentivo, out var mensajes);
            return Json(new { success = ok, mensajes });
        }


        [HttpPost]
        public async Task<IActionResult> RegistrarPagoMultiple()
        {
            var form = Request.Form;

            var detalles =
                Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<DetallePagoIncentivo>>(
                    form["Detalles"]
                );


            if (detalles == null || !detalles.Any())
                return Json(new
                {
                    success = false,
                    mensaje = "Debe agregar pagos"
                });


            string carpeta =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "ComprobantesIncentivo"
                );


            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);


            for (int i = 0; i < detalles.Count; i++)
            {
                var file = form.Files[$"Pagos[{i}].Comprobante"];

                if (file != null)
                {
                    string nombre =
                        Guid.NewGuid() +
                        Path.GetExtension(file.FileName);

                    string ruta =
                        Path.Combine(carpeta, nombre);

                    using var stream =
                        new FileStream(ruta, FileMode.Create);

                    await file.CopyToAsync(stream);

                    detalles[i].RutaComprobante =
                        nombre;
                }

                detalles[i].UsuarioPago =
                    HttpContext.Session.GetString(
                        "NombreCompleto")
                    ?? "UsuarioDesconocido";
            }



            PagoIncentivo pago = new PagoIncentivo
            {
                IdIncentivo =
                    int.Parse(form["IdIncentivo"]),

                MontoTotal =
                    detalles.Sum(x => x.MontoPagado),

                Observacion = "",

                Detalles = detalles
            };


            CN_Incentivo obj = new CN_Incentivo();

            bool ok =
                obj.RegistrarPago(
                    pago,
                    out List<string> mensajes,
                    out string numeroDocumento
                );

            return Json(new
            {
                success = ok,
                mensaje = string.Join("<br>", mensajes),
                numeroDocumento
            });
        }

        public IActionResult prueba()
        {
            return View();
        }

    }
}
