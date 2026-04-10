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
    public static class ReporteComprasClientes
    {
        // 🔹 Generar PDF
        public static byte[] GenerarPdfComprasCliente(List<ComprasCliente> lista, string accountNumber, DateTime fechaInicio, DateTime fechaFin)
        {
            string nombreCliente = lista.Count > 0 ? lista[0].Nombre : "";

            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // ===== Encabezado con logo =====
                Table header = new Table(new float[] { 1, 0.7F, 3 }).UseAllAvailableWidth();

                string rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/SABA.jpg");
                Image logo = new Image(ImageDataFactory.Create(rutaLogo)).ScaleToFit(90, 90);

                Cell logoCell = new Cell().Add(logo).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);

                Paragraph farmacia = new Paragraph("FARMACIA SABA")
                .SetFont(bold)
                .SetFontSize(14);

                Paragraph reporte = new Paragraph("REPORTE DE COMPRAS POR CLIENTE")
                    .SetFont(bold)
                    .SetFontSize(13)
                    .SetMarginTop(8);

                Div textoHeader = new Div()
                    .SetTextAlignment(TextAlignment.CENTER);

                textoHeader.Add(farmacia);
                textoHeader.Add(reporte);
                Cell textoCell = new Cell().Add(textoHeader).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                Cell espacio = new Cell().SetBorder(Border.NO_BORDER);

                header.AddCell(logoCell).AddCell(textoCell).AddCell(espacio);
                document.Add(header);

                // ===== Datos Cliente =====
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Cliente: {nombreCliente}").SetFont(bold));
                document.Add(new Paragraph($"Identidad: {accountNumber}"));
                document.Add(new Paragraph($"Periodo: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Cantidad de Compras: {lista.Count}").SetFont(bold));
                document.Add(new Paragraph(" "));

                // ===== Tabla =====
                Table table = new Table(4).UseAllAvailableWidth();
                table.AddHeaderCell(new Cell().Add(new Paragraph("Farmacia").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Fecha").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Transacción").SetFont(bold)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total").SetFont(bold)));

                decimal total = 0;
                foreach (var item in lista)
                {
                    table.AddCell(item.Farmacia);
                    table.AddCell(item.Time.ToString("dd/MM/yyyy HH:mm"));
                    table.AddCell(item.TransactionNumber);
                    table.AddCell(item.Total.ToString("C"));
                    total += item.Total;
                }

                document.Add(table);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Total Comprado: {total:C}").SetFont(bold).SetTextAlignment(TextAlignment.RIGHT));

                document.Close();
                return ms.ToArray();
            }
        }

        // 🔹 Generar Excel
        public static byte[] GenerarExcelComprasCliente(List<ComprasCliente> lista, string accountNumber, DateTime fechaInicio, DateTime fechaFin)
        {
            string nombreCliente = lista.Count > 0 ? lista[0].Nombre : "";

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Compras Cliente");

                // ===== Título =====
                ws.Cell(1, 1).Value = "REPORTE DE COMPRAS DEL CLIENTE";
                ws.Range(1, 1, 1, 4).Merge();
                ws.Range(1, 1, 1, 4).Style.Font.Bold = true;
                ws.Range(1, 1, 1, 4).Style.Font.FontSize = 16;
                ws.Range(1, 1, 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // ===== Datos Cliente =====
                ws.Cell(2, 1).Value = $"Cliente: {nombreCliente}";
                ws.Range(2, 1, 2, 4).Merge();

                ws.Cell(3, 1).Value = $"Cuenta: {accountNumber}";
                ws.Range(3, 1, 3, 4).Merge();

                ws.Cell(4, 1).Value = $"Periodo: {fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}";
                ws.Range(4, 1, 4, 4).Merge();

                ws.Cell(5, 1).Value = $"Cantidad de Compras: {lista.Count}";
                ws.Range(5, 1, 5, 4).Merge();

                // Estilo
                ws.Range(2, 1, 5, 4).Style.Font.Bold = true;
                ws.Range(2, 1, 5, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                // ===== Encabezados =====
                ws.Cell(7, 1).Value = "Farmacia";
                ws.Cell(7, 2).Value = "Fecha";
                ws.Cell(7, 3).Value = "Transacción";
                ws.Cell(7, 4).Value = "Total";

                var header = ws.Range(7, 1, 7, 4);
                header.Style.Font.Bold = true;
                header.Style.Fill.BackgroundColor = XLColor.Green;
                header.Style.Font.FontColor = XLColor.White;
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int fila = 8;
                decimal totalGeneral = 0;
                foreach (var item in lista)
                {
                    ws.Cell(fila, 1).Value = item.Farmacia;
                    ws.Cell(fila, 2).Value = item.Time.ToString("dd/MM/yyyy HH:mm");
                    ws.Cell(fila, 3).Value = item.TransactionNumber;
                    ws.Cell(fila, 4).Value = item.Total;
                    totalGeneral += item.Total;
                    fila++;
                }

                ws.Cell(fila, 3).Value = "Total Comprado:";
                ws.Cell(fila, 4).Value = totalGeneral;
                ws.Range(fila, 3, fila, 4).Style.Font.Bold = true;
                ws.Range(fila, 3, fila, 4).Style.Fill.BackgroundColor = XLColor.LightGray;

                ws.Column(4).Style.NumberFormat.Format = "C$ #,##0.00";
                ws.Range(7, 1, fila, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(7, 1, fila, 4).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

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
