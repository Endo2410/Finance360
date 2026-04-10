using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using static Capa_Presentacion.Controllers.IncentivoController;

namespace Capa_Presentacion.Utilidades
{
    public static class ReporteCitasUtil
    {
        public static byte[] GenerarPdf(List<CitaExportDTO> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document doc = new Document(pdf);

                // =========================
                // FUENTES
                // =========================
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // =========================
                // COLORES
                // =========================
                var verde = new DeviceRgb(102, 187, 106);
                var grisClaro = new DeviceRgb(240, 240, 240);

                // =========================
                // HEADER
                // =========================
                Table header = new Table(2).UseAllAvailableWidth();

                string rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/SABA.jpg");

                if (File.Exists(rutaLogo))
                {
                    Image logo = new Image(ImageDataFactory.Create(rutaLogo))
                        .ScaleToFit(80, 80);

                    header.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    header.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
                }

                header.AddCell(new Cell()
                    .Add(new Paragraph("REPORTE DE CITAS MÉDICAS")
                        .SetFont(bold)
                        .SetFontSize(16)
                        .SetFontColor(verde))
                    .Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}")
                        .SetFont(normal)
                        .SetFontSize(10))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER));

                doc.Add(header);
                doc.Add(new Paragraph("\n"));

                // =========================
                // AGRUPAR
                // =========================
                var agrupado = lista.GroupBy(x => x.Sucursal);

                foreach (var grupo in agrupado)
                {
                    bool primerCard = true;

                    foreach (var item in grupo)
                    {
                        // TITULO
                        var titulo = new Paragraph(grupo.Key.ToUpper())
                            .SetFont(bold)
                            .SetFontSize(12)
                            .SetBackgroundColor(verde)
                            .SetFontColor(ColorConstants.WHITE)
                            .SetPadding(3)
                            .SetMarginBottom(5);

                        // CARD
                        Div card = new Div()
                            .SetBackgroundColor(grisClaro)
                            .SetPadding(10)
                            .SetMarginBottom(8)
                            .SetBorderRadius(new BorderRadius(5))
                            .SetKeepTogether(true);

                        card.Add(new Paragraph(item.Nombre)
                            .SetFont(bold)
                            .SetFontSize(11));

                        card.Add(new Paragraph("Departamento: " + item.Departamento)
                            .SetFont(normal)
                            .SetFontSize(10));

                        var fechas = item.Fechas
                            .Split(',')
                            .Select(f => DateTime.Parse(f).ToString("dd/MM/yyyy"));

                        card.Add(new Paragraph("Fechas:")
                            .SetFont(bold)
                            .SetFontSize(10));

                        foreach (var f in fechas)
                        {
                            card.Add(new Paragraph("✔ " + f)
                                .SetFont(normal)
                                .SetFontSize(9));
                        }

                        if (primerCard)
                        {
                            Div bloque = new Div().SetKeepTogether(true);
                            bloque.Add(titulo);
                            bloque.Add(card);

                            doc.Add(bloque);
                            primerCard = false;
                        }
                        else
                        {
                            doc.Add(card);
                        }
                    }
                }

                doc.Close();
                return ms.ToArray();
            }
        }

        
    }

    public class CitaExportDTO
    {
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string Fechas { get; set; }
    }
}