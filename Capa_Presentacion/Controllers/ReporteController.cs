using Capa_Entidad;
using Capa_Negocio;

using Microsoft.AspNetCore.Mvc;

using Capa_Presentacion.Utilidades;
using ClosedXML.Excel;

namespace Capa_Presentacion.Controllers
{
    public class ReporteController : Controller
    {
        private readonly CN_Reportes objcn = new();

        //cliente
        public IActionResult ComprasCliente(string accountNumber, DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue)
                fechaInicio = DateTime.Today.AddMonths(-1);

            if (!fechaFin.HasValue)
                fechaFin = DateTime.Today;

            List<ComprasCliente> lista = new();

            if (!string.IsNullOrEmpty(accountNumber))
            {
                lista = objcn.ObtenerComprasCliente(
                    accountNumber,
                    fechaInicio.Value,
                    fechaFin.Value
                );
            }

            ViewBag.AccountNumber = accountNumber;
            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");

            return View(lista);
        }

        public IActionResult DescargarPdf(string accountNumber, DateTime fechaInicio, DateTime fechaFin, string filtroCol, string filtroVal)
        {
            var lista = objcn.ObtenerComprasCliente(accountNumber, fechaInicio, fechaFin);
            lista = AplicarFiltros(lista, filtroCol, filtroVal);

            byte[] pdf = ReporteComprasClientes.GenerarPdfComprasCliente(lista, accountNumber, fechaInicio, fechaFin);

            Response.Headers.Add("Content-Disposition", "inline; filename=ComprasCliente.pdf");

            return File(pdf, "application/pdf");
        }

        public IActionResult ExportarExcelComprasCliente(string accountNumber, DateTime fechaInicio, DateTime fechaFin, string filtroCol, string filtroVal)
        {
            var lista = objcn.ObtenerComprasCliente(accountNumber, fechaInicio, fechaFin);
            lista = AplicarFiltros(lista, filtroCol, filtroVal);

            byte[] excel = ReporteComprasClientes.GenerarExcelComprasCliente(lista, accountNumber, fechaInicio, fechaFin);
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ComprasCliente.xlsx");
        }

        private List<ComprasCliente> AplicarFiltros(List<ComprasCliente> lista, string filtroCol, string filtroVal)
        {
            if (!string.IsNullOrEmpty(filtroCol) && !string.IsNullOrEmpty(filtroVal))
            {
                switch (filtroCol)
                {
                    case "0": lista = lista.Where(x => x.Farmacia.Contains(filtroVal, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                    case "1": lista = lista.Where(x => x.Time.ToString("dd/MM/yyyy").Contains(filtroVal)).ToList(); break;
                    case "2": lista = lista.Where(x => x.TransactionNumber.Contains(filtroVal, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                    case "3": lista = lista.Where(x => x.Nombre.Contains(filtroVal, StringComparison.OrdinalIgnoreCase)).ToList(); break;
                }
            }
            return lista;
        }



        ///para ver reporte de compa de los proveedores
        public IActionResult ReporteProveedores(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string laboratorio)
        {
            if (!fechaInicio.HasValue)
                fechaInicio = DateTime.Today.AddMonths(-2);

            if (!fechaFin.HasValue)
                fechaFin = DateTime.Today;

            var lista = objcn.ObtenerReporte(
                fechaInicio.Value,
                fechaFin.Value,
                string.IsNullOrWhiteSpace(proveedor) ? null : proveedor,
                string.IsNullOrWhiteSpace(laboratorio) ? null : laboratorio
            );

            ViewBag.TotalGeneral = lista.Sum(x => x.TotalComprado);

            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");
            ViewBag.Proveedor = proveedor;
            ViewBag.Laboratorio = laboratorio;

            return View(lista);
        }

        public IActionResult DescargarPdfCompras(DateTime fechaInicio, DateTime fechaFin, int? campo, string valor)
        {
            string proveedorFiltro = null;
            string laboratorioFiltro = null;

            if (!string.IsNullOrEmpty(valor) && campo.HasValue)
            {
                valor = valor.ToLower();

                if (campo == 0) proveedorFiltro = valor;
                if (campo == 1) laboratorioFiltro = valor;
            }

            var lista = objcn.ObtenerReporte(fechaInicio, fechaFin, proveedorFiltro, laboratorioFiltro);

            byte[] pdf = ReporteComprasUtil.GenerarPdf(lista, fechaInicio, fechaFin);

            Response.Headers.Add("Content-Disposition", "inline; filename=ReporteCompras.pdf");

            return File(pdf, "application/pdf");
        }

        public IActionResult ExportarExcelCompras(DateTime fechaInicio, DateTime fechaFin, int? campo, string valor)
        {
            string proveedorFiltro = null;
            string laboratorioFiltro = null;

            if (!string.IsNullOrEmpty(valor) && campo.HasValue)
            {
                valor = valor.ToLower();

                if (campo == 0) proveedorFiltro = valor;
                if (campo == 1) laboratorioFiltro = valor;
            }

            var lista = objcn.ObtenerReporte(fechaInicio, fechaFin, proveedorFiltro, laboratorioFiltro);

            byte[] excel = ReporteComprasUtil.GenerarExcel(lista, fechaInicio, fechaFin);

            return File(excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ReporteCompras.xlsx"
            );
        }


        public IActionResult OrdenesSinRecibir(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue)
                fechaInicio = DateTime.Today.AddMonths(-2);

            if (!fechaFin.HasValue)
                fechaFin = DateTime.Today;

            var lista = objcn.ObtenerOrdenessinrecibir(
                fechaInicio.Value,
                fechaFin.Value
            );

            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");


            return View(lista);
        }

        public IActionResult DescargarPdfOrdenes(DateTime fechaInicio, DateTime fechaFin, int? campo, string valor)
        {
            var lista = objcn.ObtenerOrdenessinrecibir(fechaInicio, fechaFin);

            if (!string.IsNullOrEmpty(valor) && campo.HasValue)
            {
                valor = valor.ToLower();

                if (campo == 0)
                    lista = lista.Where(x => (x.PONumber ?? "").ToLower().Contains(valor)).ToList();

                if (campo == 1)
                    lista = lista.Where(x => (x.Farmacia ?? "").ToLower().Contains(valor)).ToList();
            }

            byte[] pdf = ReporteOrdenesUtil.GenerarPdf(lista, fechaInicio, fechaFin);

            Response.Headers.Add("Content-Disposition", "inline; filename=OrdenesSinRecibir.pdf");

            return File(pdf, "application/pdf");
        }

        public IActionResult ExportarExcelOrdenes(DateTime fechaInicio, DateTime fechaFin, int? campo, string valor)
        {
            var lista = objcn.ObtenerOrdenessinrecibir(fechaInicio, fechaFin);

            if (!string.IsNullOrEmpty(valor) && campo.HasValue)
            {
                valor = valor.ToLower();

                if (campo == 0)
                    lista = lista.Where(x => (x.PONumber ?? "").ToLower().Contains(valor)).ToList();

                if (campo == 1)
                    lista = lista.Where(x => (x.Farmacia ?? "").ToLower().Contains(valor)).ToList();
            }

            byte[] excel = ReporteOrdenesUtil.GenerarExcel(lista, fechaInicio, fechaFin);

            return File(excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "OrdenesSinRecibir.xlsx");
        }
    }
}
