using Capa_Entidad;
using ClosedXML.Excel;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;

using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;



namespace Capa_Presentacion.Utilidades
{
    public static class ReporteComprasUtil
    {

        //========================
        // PDF
        //========================
        public static byte[] GenerarPdf(List<ReporteCompras> lista, DateTime fechaInicio, DateTime fechaFin)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);


                // ===== Encabezado =====
                Table header = new Table(new float[] { 1, 3, 1 }).UseAllAvailableWidth();

                // ===== Logo =====
                string rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/SABA.jpg");

                Image logo = new Image(ImageDataFactory.Create(rutaLogo))
                    .ScaleToFit(80, 80)
                    .SetFixedPosition(40, 750); // izquierda, altura

                document.Add(logo);

                // ===== Títulos =====
                document.Add(
                    new Paragraph("FARMACIA SABA")
                        .SetFont(bold)
                        .SetFontSize(14)
                        .SetTextAlignment(TextAlignment.CENTER)
                );

                document.Add(
                    new Paragraph("REPORTE DE COMPRAS")
                        .SetFont(bold)
                        .SetFontSize(13)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5)
                );


                document.Add(header);

                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Periodo: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Cantidad de registros: {lista.Count}").SetFont(bold));
                document.Add(new Paragraph(" "));

                // ===== Tabla =====
                Table table = new Table(3).UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("Proveedor").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Laboratorio").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total Comprado").SetFont(bold)));

                decimal total = 0;

                foreach (var item in lista)
                {
                    table.AddCell(item.Proveedor);
                    table.AddCell(item.Laboratorio);
                    table.AddCell(item.TotalComprado.ToString("C"));

                    total += item.TotalComprado;
                }

                document.Add(table);

                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Total General: {total:C}")
                    .SetFont(bold)
                    .SetTextAlignment(TextAlignment.RIGHT));

                document.Close();

                return ms.ToArray();
            }
        }

        //========================
        // EXCEL
        //========================
        public static byte[] GenerarExcel(List<ReporteCompras> lista, DateTime fechaInicio, DateTime fechaFin)
        {

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Reporte Compras");

                ws.Cell(1, 1).Value = "REPORTE DE COMPRAS";
                ws.Range(1, 1, 1, 3).Merge();

                ws.Range(1, 1, 1, 3).Style.Font.Bold = true;
                ws.Range(1, 1, 1, 3).Style.Font.FontSize = 16;
                ws.Range(1, 1, 1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(2, 1).Value = $"Periodo: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}";
                ws.Range(2, 1, 2, 3).Merge();

                ws.Cell(4, 1).Value = "Proveedor";
                ws.Cell(4, 2).Value = "Laboratorio";
                ws.Cell(4, 3).Value = "Total Comprado";

                var header = ws.Range(4, 1, 4, 3);

                header.Style.Font.Bold = true;
                header.Style.Fill.BackgroundColor = XLColor.Green;
                header.Style.Font.FontColor = XLColor.White;
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int fila = 5;

                foreach (var item in lista)
                {
                    ws.Cell(fila, 1).Value = item.Proveedor;
                    ws.Cell(fila, 2).Value = item.Laboratorio;
                    ws.Cell(fila, 3).Value = item.TotalComprado;

                    fila++;
                }

                decimal total = lista.Sum(x => x.TotalComprado);

                ws.Cell(fila, 2).Value = "Total General:";
                ws.Cell(fila, 3).Value = total;

                ws.Range(fila, 2, fila, 3).Style.Font.Bold = true;

                ws.Column(3).Style.NumberFormat.Format = "C$ #,##0.00";

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
