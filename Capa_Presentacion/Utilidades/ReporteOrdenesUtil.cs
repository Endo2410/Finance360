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
    public static class ReporteOrdenesUtil
    {

        //========================
        // PDF
        //========================
        public static byte[] GenerarPdf(List<OrdenSinRecibir> lista, DateTime fechaInicio, DateTime fechaFin)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                string rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/SABA.jpg");

                Image logo = new Image(ImageDataFactory.Create(rutaLogo))
                    .ScaleToFit(80, 80)
                    .SetFixedPosition(40, 750);

                document.Add(logo);

                document.Add(
                    new Paragraph("FARMACIA SABA")
                    .SetFont(bold)
                    .SetFontSize(14)
                    .SetTextAlignment(TextAlignment.CENTER)
                );

                document.Add(
                    new Paragraph("ÓRDENES SIN RECIBIR")
                    .SetFont(bold)
                    .SetFontSize(13)
                    .SetTextAlignment(TextAlignment.CENTER)
                );

                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Periodo: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Cantidad de registros: {lista.Count}").SetFont(bold));
                document.Add(new Paragraph(" "));

                Table table = new Table(3).UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("PO Number").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Farmacia").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Fecha").SetFont(bold)));

                foreach (var item in lista)
                {
                    table.AddCell(item.PONumber);
                    table.AddCell(item.Farmacia);
                    table.AddCell(item.DateCreated.ToString("dd/MM/yyyy"));
                }

                document.Add(table);

                document.Close();

                return ms.ToArray();
            }
        }

        //========================
        // EXCEL
        //========================
        public static byte[] GenerarExcel(List<OrdenSinRecibir> lista, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Ordenes");

                ws.Cell(1, 1).Value = "ÓRDENES SIN RECIBIR";
                ws.Range(1, 1, 1, 3).Merge();

                ws.Range(1, 1, 1, 3).Style.Font.Bold = true;
                ws.Range(1, 1, 1, 3).Style.Font.FontSize = 16;
                ws.Range(1, 1, 1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Cell(2, 1).Value = $"Periodo: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}";
                ws.Range(2, 1, 2, 3).Merge();

                ws.Cell(4, 1).Value = "PO Number";
                ws.Cell(4, 2).Value = "Farmacia";
                ws.Cell(4, 3).Value = "Fecha";

                var header = ws.Range(4, 1, 4, 3);

                header.Style.Font.Bold = true;
                header.Style.Fill.BackgroundColor = XLColor.Green;
                header.Style.Font.FontColor = XLColor.White;
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int fila = 5;

                foreach (var item in lista)
                {
                    ws.Cell(fila, 1).Value = item.PONumber;
                    ws.Cell(fila, 2).Value = item.Farmacia;
                    ws.Cell(fila, 3).Value = item.DateCreated;

                    fila++;
                }

                ws.Column(3).Style.DateFormat.Format = "dd/MM/yyyy";

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