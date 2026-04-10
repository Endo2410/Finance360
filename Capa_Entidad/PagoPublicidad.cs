using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class PagoPublicidad
    {
        public int IdCampania { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaDocumento { get; set; }

        // 🔥 CLARO Y PROFESIONAL
        public decimal MontoNeto { get; set; }
        public decimal MontoRetenciones { get; set; }
        public decimal MontoBruto { get; set; }

        public string Observacion { get; set; }

        public List<DetallePagoPublicidad> DetalleCuotas { get; set; }
            = new List<DetallePagoPublicidad>();

        public List<RetencionVM> Retenciones { get; set; }
            = new List<RetencionVM>();

    }



    public class RetencionVM
    {
        public int IdTipoRetencion { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal MontoRetenido { get; set; }

    }
}
