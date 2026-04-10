using Capa_Entidad;
using Capa_Entidad.CE_Rebate;
using Capa_Negocio;
using Capa_Negocio.Rebate;
using Capa_Presentacion.Filtros;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Capa_Presentacion.Controllers
{
    //[FiltroSesion]
    public class RebateController : Controller
    {
        #region DEPENDECIAS
        private readonly CN_EjecucionRebate cnejecucion = new CN_EjecucionRebate();
        private readonly CN_AcuerdoRebate cnacuerdo = new CN_AcuerdoRebate();
        private readonly CN_CriterioRebate cnciterio = new();
        private readonly CN_PagoRebate objPago = new();
        private readonly CN_TipoRebate cntipo = new();    
        private readonly CN_Estado cnEstado = new();
        #endregion

        #region ACUERDO_REBATE
        public IActionResult Acuerdo_Rebate()
        {
            var lista = cnacuerdo.ObtenerAcuerdos();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpGet]
        public IActionResult Listar(int? tipoRebate = null)
        {
            try
            {
                var hoy = DateTime.Today;

                var lista = cnacuerdo.ObtenerAcuerdos()
                    .Where(a => a.oEstado.IdEstado == 3) // solo creados
                    .Where(a =>
                        // 🔥 SI ES TIPO 3 o 4 → NO VALIDAR FECHA
                        (a.oTipoRebate.IdTipoRebate == 3 || a.oTipoRebate.IdTipoRebate == 4)

                        // 🔥 OTROS TIPOS → VALIDAR FECHA
                        || (hoy >= a.FechaInicio && hoy <= a.FechaFin)
                    )
                    .ToList();

                return Json(lista);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerDetalles(int idAcuerdo)
        {
            try
            {
                var detalles = cnacuerdo.ObtenerDetalles(idAcuerdo);
                return Json(detalles);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Crear([FromForm] AcuerdoRebate acuerdo, IFormFile archivo)
        {
            var detalleJson = Request.Form["detalleItems"];

            List<DetalleAcuerdo> detalles = new();

            if (!string.IsNullOrEmpty(detalleJson))
            {
                detalles = JsonSerializer.Deserialize<List<DetalleAcuerdo>>(detalleJson,
                 new JsonSerializerOptions
                 {
                     PropertyNameCaseInsensitive = true
                 });
            }

            // Carpeta donde se guardarán los archivos
            string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "acuerdos");
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            if (archivo != null && archivo.Length > 0)
            {
                // Generar un nombre único para el archivo
                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                // Guardar el nombre del archivo en el objeto
                acuerdo.Documento = nombreArchivo;
            }

            // Obtener el usuario logueado
            acuerdo.UsuarioCreacion = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            // Llamada al CN
           bool exito = cnacuerdo.CrearAcuerdo(acuerdo, detalles, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Acuerdo de rebate creado correctamente." };

            return Json(new { success = exito, mensajes });
        }

        [HttpPost]
        public IActionResult Editar([FromForm] AcuerdoRebate acuerdo, IFormFile archivo)
        {
            // 📌 Obtener detalles del form
            var detalleJson = Request.Form["detalleItems"];
            List<DetalleAcuerdo> detalles = new();
            if (!string.IsNullOrEmpty(detalleJson))
            {
                detalles = JsonSerializer.Deserialize<List<DetalleAcuerdo>>(detalleJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            // Carpeta de archivos
            string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "acuerdos");
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            // Archivo
            if (archivo != null && archivo.Length > 0)
            {
                string nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                archivo.CopyTo(stream);
                acuerdo.Documento = nombreArchivo;
            }
            else
            {
                acuerdo.Documento = Request.Form["DOCUMENTO_EXISTENTE"];
            }

            acuerdo.UsuarioModificacion = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

            // Llamada a negocio
            bool exito = cnacuerdo.EditarAcuerdo(acuerdo, detalles, out List<string> mensajes);

            if (exito && (mensajes == null || mensajes.Count == 0))
                mensajes = new List<string> { "Acuerdo de rebate actualizado correctamente." };

            return Json(new { success = exito, mensajes });
        }
        #endregion

        public IActionResult Gestion_Ejecuciones()
        {
            return View();
        }

        public IActionResult Ejecucion_Descuento()
        {
            var lista = cnejecucion.ObtenerEjecucionesDescuento();

            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto")
                                    ?? "UsuarioDesconocido";

            return View(lista);
        }

       [HttpGet]
        public JsonResult ObtenerDetalleEjecucionDescuento(int idEjecucion)
        {
            var lista = cnejecucion.ObtenerDetalleEjecucionDescuento(idEjecucion);
            return Json(lista);
        }

        #region EJECUCION_REBATE
        public IActionResult Ejecucion_Rebate()
        {
            var lista = cnejecucion.ObtenerEjecuciones();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpPost]
        public JsonResult GuardarEjecucion(EjecucionRebate obj, IFormFile fileSoporte)
        {
            string mensaje;
            string nombreArchivo = null;

            try
            {
                // Guardar archivo físicamente si existe
                if (fileSoporte != null && fileSoporte.Length > 0)
                {
                    string carpeta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "archivo",
                        "rebate"
                    );

                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    // Nombre único con GUID + extensión original
                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileSoporte.FileName);
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        fileSoporte.CopyTo(stream);
                    }
                }

                //  Asignar archivo y usuario 
                obj.ArchivoSoporte = nombreArchivo;
                obj.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                // 3️⃣ Guardar ejecución
                bool resultado = cnejecucion.CrearEjecucion(obj, out mensaje);

                return Json(new { resultado, mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EditarEjecucion(EjecucionRebate obj, IFormFile fileSoporte)
        {
            string mensaje;
            string nombreArchivo = obj.ArchivoSoporte; // Mantener archivo anterior si no se envía uno nuevo

            try
            {
                // Guardar archivo nuevo si existe
                if (fileSoporte != null && fileSoporte.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "rebate");

                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileSoporte.FileName);
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        fileSoporte.CopyTo(stream);
                    }
                }

                obj.ArchivoSoporte = nombreArchivo;

                // AQUÍ PASAS EL USUARIO QUE MODIFICA
                obj.UsuarioModificacion = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";


                // Llamar SP para editar ejecución
                bool resultado = cnejecucion.EditarEjecucion(obj, out mensaje);

                return Json(new { resultado, mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }
        #endregion

        #region EJECUCION_ITEM
        public IActionResult Ejecucion_Item()
        {
            var lista = cnejecucion.ObtenerEjecucionesItem();

            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto")
                                    ?? "UsuarioDesconocido";

            return View(lista);
        }

        [HttpGet]
        public JsonResult ObtenerDetalleEjecucion(int idEjecucion)
        {
            var lista = cnejecucion.ObtenerDetalleEjecucion(idEjecucion);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult GuardarEjecucionItem(EjecucionRebate obj, IFormFile fileSoporte, string detalles)
        {
            string mensaje;
            string nombreArchivo = null;
            int idEjecucion = 0;

            try
            {
                // 📁 Guardar archivo
                if (fileSoporte != null && fileSoporte.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "rebate");

                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileSoporte.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        fileSoporte.CopyTo(stream);
                    }
                }

                obj.ArchivoSoporte = nombreArchivo;
                obj.UsuarioRegistro = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                // 🔥 Guardar TODO (padre + detalle)
                bool resultado = cnejecucion.CrearEjecucionItemCompleto(obj, detalles, out idEjecucion, out mensaje);

                return Json(new { resultado, mensaje, idEjecucion });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EditarEjecucionItem(EjecucionRebate obj, string detalles, IFormFile fileSoporte)
        {
            string mensaje;
            string nombreArchivo = obj.ArchivoSoporte;

            try
            {
                if (fileSoporte != null && fileSoporte.Length > 0)
                {
                    string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivo", "rebate");

                    nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(fileSoporte.FileName);

                    using (var stream = new FileStream(Path.Combine(carpeta, nombreArchivo), FileMode.Create))
                    {
                        fileSoporte.CopyTo(stream);
                    }
                }

                obj.ArchivoSoporte = nombreArchivo;
                obj.UsuarioModificacion = HttpContext.Session.GetString("NombreCompleto");

                bool resultado = cnejecucion.EditarEjecucionItem(obj, detalles, out mensaje);

                return Json(new { resultado, mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }
       #endregion

        #region LISTAR_PAGO
        public IActionResult ListaPagosRebate(string doc = null)
        {
            var lista = cnejecucion.ObtenerEjecucionesRebateResumen();

            // Pasar filtro desde Notas de Crédito si existe
            ViewBag.FiltroDocumento = doc;

            return View(lista);

        }

        [HttpGet]
        public IActionResult DetallePagoEjecucion(int id)
        {
            try
            {
                var detalles = cnejecucion.ObtenerDetallePagoEjecucionRebate(id);
                return Json(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AnularPagoEjecucionRebate(int idDetallePago)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";

                bool resultado = cnejecucion.AnularPagoEjecucionRebate(idDetallePago, usuario);

                if (!resultado)
                {
                    return Json(new
                    {
                        success = false,
                        mensaje = "No se pudo anular el pago."
                    });
                }

                return Json(new
                {
                    success = true,
                    mensaje = "Pago anulado correctamente."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    mensaje = ex.Message
                });
            }
        }
        #endregion

        #region PAGOS_REBATE
        public IActionResult Pago_Rebate()
        {
            var lista = cnejecucion.ObtenerEjecuciones();
            ViewBag.UsuarioActual = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            return View(lista);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPagoMultiples()
        {
            var form = Request.Form;

            var detalleEjecuciones = Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<DetallePagoRebate>>(form["DetalleEjecuciones"]);

            if (!decimal.TryParse(form["TotalAPagar"], out decimal totalAPagar))
                return Json(new { success = false, mensajes = new[] { "Total a pagar inválido" } });

            if (detalleEjecuciones == null || !detalleEjecuciones.Any())
                return Json(new { success = false, mensajes = new[] { "Debe seleccionar al menos una ejecución" } });

            if (detalleEjecuciones.Any(d => d.MontoPagado <= 0))
                return Json(new { success = false, mensajes = new[] { "Los montos deben ser mayores a cero" } });

            // Guardar comprobantes
            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ComprobantesRebate");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            for (int i = 0; i < detalleEjecuciones.Count; i++)
            {
                var file = form.Files[$"PagosTipos[{i}].Comprobante"];
                if (file != null && file.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);
                    using var stream = new FileStream(ruta, FileMode.Create);
                    await file.CopyToAsync(stream);

                    detalleEjecuciones[i].RutaComprobante = nombreArchivo;
                }

                // ✅ Asignar el usuario que registra el pago
                detalleEjecuciones[i].usuarioPago = HttpContext.Session.GetString("NombreCompleto") ?? "UsuarioDesconocido";
            }

            var retenciones = Newtonsoft.Json.JsonConvert
             .DeserializeObject<List<RetencionVM>>(form["Retenciones"]) ?? new List<RetencionVM>();

            PagoRebate pago = new()
            {
                IdAcuerdo = int.Parse(form["IdAcuerdo"]),
                FechaDocumento = DateTime.Now,
                Observacion = form["Observacion"],
                MontoTotal = totalAPagar,
                DetalleEjecuciones = detalleEjecuciones,
                Retenciones = retenciones
            };

            bool exito = objPago.RegistrarPago(pago, out List<string> mensajes, out string numeroDocumento);
            return Json(new { success = exito, mensajes, numeroDocumento });
        }
        #endregion

        #region CRITERIO_REBATE
        public IActionResult Criterio_Rebate()
        {
            return View(cnciterio.ObtenerCriterios());
        }

        public IActionResult ListarCriterio()
        {
            var lista = cnciterio.ObtenerCriterios();
            return Json(lista.Select(t => new { id = t.IdCriterio, nombre = t.Nombre }));
        }

        [HttpPost]
        public IActionResult CrearCriterio(CriterioRebate obj)
        {
            bool ok = cnciterio.Crear(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarCriterio(CriterioRebate obj)
        {
            bool ok = cnciterio.Editar(obj, out List<string> mensajes);
            return Json(new { success = ok, mensajes });
        }
        #endregion

        #region TIPOS_REBATE
        public IActionResult Tipo_Rebate()
        {
            return View(cntipo.Obtener());
        }

        // Listar Tipos de rebate
        public IActionResult ListarTipo()
        {
            var lista = cntipo.Obtener();
            return Json(lista.Select(t => new { id = t.IdTipoRebate, nombre = t.Nombre }));
        }

        [HttpPost]
        public IActionResult CrearTipo(TipoRebate obj)
        {
            bool ok = cntipo.Crear(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }

        [HttpPost]
        public IActionResult EditarTipo(TipoRebate obj)
        {
            bool ok = cntipo.Editar(obj, out var mensajes);
            return Json(new { success = ok, mensajes });
        }
        #endregion
    }
}
