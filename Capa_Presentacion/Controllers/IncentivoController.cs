using Capa_Entidad.CE_Incentivo;
using Capa_Negocio.Incentivo;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using Capa_Presentacion.Utilidades;
using Capa_Entidad;
using Capa_Negocio;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class IncentivoController : Controller
    {
        #region DEPENDECIAS
        private readonly CN_Incentivo cn = new();
        private readonly CN_CitaMedica cncitas = new CN_CitaMedica();
        private readonly CN_IncentivoSaldo cnincentivo = new CN_IncentivoSaldo();
        private readonly CN_TipoIncentivo cntipos = new();
        private readonly CN_Tipousoincentivo cnuso = new();
        private readonly CN_Estado cnEstado = new();
        private readonly CN_PorcentajeComisiones cnporcentaje = new();
        #endregion

        #region GESTION_INCENTIVO
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

        [HttpGet]
        public IActionResult ObtenerDetallePago(int id)
        {
            var lista = cn.ObtenerDetallePago(id);

            return Json(lista.Select(x => new
            {
                tipoDocumento = x.TipoDocumento,
                monto = x.MontoPagado,
                confirmacion = x.NumeroConfirmacion,
                comprobante = x.RutaComprobante,
                usuario = x.UsuarioPago,
                fecha = x.FechaRegistro.ToString("dd/MM/yyyy")
            }));
        }
        #endregion

        public IActionResult prueba()
        {
            return View();
        }

        #region INSENTIVO_MOVIMIENTO
        public IActionResult IncentivoSaldo()
        {
            return View();
        }

        public IActionResult IncentivosRecibidos()
        {
            return View();
        }
        public IActionResult UsoIncentivo()
        {
            return View();
        }


        // SALDO
        [HttpGet]
        public IActionResult ObtenerSaldo(int idSucursal)
        {
            int? sucursalSesion = HttpContext.Session.GetInt32("IdSucursal");

            if (sucursalSesion != null)
                idSucursal = sucursalSesion.Value;

            var saldo = cnincentivo.ObtenerSaldo(idSucursal);

            return Json(saldo);
        }

        // INCENTIVOS RECIBIDOS
        [HttpGet]
        public JsonResult ObtenerIncentivosRecibidos(int idSucursal)
        {
            int? sucursalSesion = HttpContext.Session.GetInt32("IdSucursal");

            if (sucursalSesion != null)
                idSucursal = sucursalSesion.Value;

            var lista = cnincentivo.ObtenerIncentivosRecibidos(idSucursal);

            return Json(new { data = lista });
        }

        // USOS
        [HttpGet]
        public IActionResult ObtenerUsos(int idSucursal)
        {
            int? sucursalSesion = HttpContext.Session.GetInt32("IdSucursal");

            if (sucursalSesion != null)
                idSucursal = sucursalSesion.Value;

            var lista = cnincentivo.ObtenerUsos(idSucursal);

            return Json(new { data = lista });
        }

        // COLABORADORES
        [HttpGet]
        public IActionResult ObtenerColaboradores(int idSucursal)
        {
            int? sucursalSesion = HttpContext.Session.GetInt32("IdSucursal");

            if (sucursalSesion != null)
                idSucursal = sucursalSesion.Value;

            var lista = cnincentivo.ObtenerColaboradores(idSucursal);

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

                // NUEVO: recibir colaboradores desde la vista
                string colaboradores = form["colaboradores"];

                IncentivoMovimiento obj = new IncentivoMovimiento()
                {
                    IdSucursal = Convert.ToInt32(form["idsucursal"]),
                    IdTipoUso = Convert.ToInt32(form["idtipouso"]),
                    Monto = Convert.ToDecimal(form["monto"]),
                    Observacion = form["observacion"],

                    UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido",

                    Comprobante = nombreArchivo,

                    ColaboradoresJson = colaboradores
                };

                bool respuesta = cnincentivo.RegistrarUsoIncentivo(obj);

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

        [HttpGet]
        public IActionResult ObtenerDetalleColaboradores(int idMovimiento)
        {
            var lista = cnincentivo.ObtenerDetalleColaboradores(idMovimiento);

            return Json(new { data = lista });
        }
        #endregion

        #region CITAS_MEDICAS
        public IActionResult CitasMedica()
        {
            return View(cncitas.Obtener());
        }

        [HttpGet]
        public IActionResult ObtenerFechas(int id)
        {
            var lista = cncitas.Obtener()
                .FirstOrDefault(x => x.IdCita == id)?.Fechas;

            return Json(lista);
        }

        [HttpPost]
        public IActionResult CrearCitasmedicas([FromForm] CitaMedica cita, string Fechas, IFormFile archivo)
        {
            string ruta = Path.Combine("wwwroot/archivo/citas");
            Directory.CreateDirectory(ruta);

            // SI SUBE NUEVO ARCHIVO
            if (archivo != null)
            {
                // eliminar anterior
                if (!string.IsNullOrEmpty(cita.DocumentoAdjunto))
                {
                    string rutaAnterior = Path.Combine(ruta, cita.DocumentoAdjunto);
                    if (System.IO.File.Exists(rutaAnterior))
                        System.IO.File.Delete(rutaAnterior);
                }

                string nombre = $"{Guid.NewGuid()}_{archivo.FileName}";

                using var fs = new FileStream(Path.Combine(ruta, nombre), FileMode.Create);
                archivo.CopyTo(fs);

                cita.DocumentoAdjunto = nombre;
            }
            // SI NO SUBE ARCHIVO → MANTENER EL MISMO
            else if (cita.IdCita > 0)
            {
                // NO tocar DocumentoAdjunto, ya viene del hidden
            }

            cita.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "Sistema";

            cita.Fechas = System.Text.Json.JsonSerializer.Deserialize<List<DateTime>>(Fechas);

            List<string> mensajes;
            bool ok;

            if (cita.IdCita > 0)
                ok = cncitas.Editar(cita, out mensajes);
            else
                ok = cncitas.Crear(cita, out mensajes);

            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult ExportarCitasPDF([FromBody] List<CitaExportDTO> lista)
        {
            var pdfBytes = ReporteCitasUtil.GenerarPdf(lista);

            return File(pdfBytes,
                "application/pdf",
                "ReporteCitasMedicas.pdf");
        }
        #endregion

        #region TIPO_INCENTIVO
        public IActionResult Tipo_Incentivo()
        {
            return View(cntipos.Obtener());
        }

        public IActionResult GestionIncentivo()
        {
            return View();
        }

        // Listar 
        public IActionResult Listar()
        {
            var lista = cntipos.Obtener();
            return Json(lista.Select(t => new { id = t.IdTipoIncentivo, nombre = t.Nombre }));
        }

        public IActionResult Estado()
        {
            var estados = cnEstado.ObtenerEstado("GENERAL", out string msg);

            if (!string.IsNullOrEmpty(msg))
                return Json(new { success = false, mensajes = new[] { msg } });

            return Json(new
            {
                success = true,
                estados = estados.Select(e => new { idEstado = e.IdEstado, nombre = e.Nombre })
            });
        }

        [HttpPost]
        public IActionResult CrearTipoIncentivo(TipoIncentivo obj)
        {
            bool ok = cntipos.Crear(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarTipoIncentivo(TipoIncentivo obj)
        {
            bool ok = cntipos.Editar(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }
        #endregion

        #region TIPO_USO
        // Listar Tipos de uso incentivo
        public IActionResult Tipouso()
        {
            return View(cnuso.ObtenerUso());
        }

        public IActionResult ListarUso()
        {
            var lista = cnuso.ObtenerUso();
            return Json(lista.Select(t => new { id = t.IdTipoUsoIncentivo, nombre = t.Nombre }));
        }

        [HttpPost]
        public IActionResult CrearUso(TipoUsoIncentivo obj)
        {
            bool ok = cnuso.CrearUso(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarUso(TipoUsoIncentivo obj)
        {
            bool ok = cnuso.EditarUso(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }
        #endregion

        #region COMISIONES_PORCENTAJE
        public IActionResult ComisionesProcentaje() => View(cnporcentaje.Obtener());

        public IActionResult ListarComisionesProcentaje()
        {
            var lista = cnporcentaje.Obtener();
            return Json(lista);
        }

        [HttpPost]
        public IActionResult CrearComisionesProcentaje(PorcentajeComisiones obj)
        {
            bool ok = cnporcentaje.Crear(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarComisionesProcentaje(PorcentajeComisiones obj)
        {
            bool ok = cnporcentaje.Editar(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }
        #endregion
    }
}
