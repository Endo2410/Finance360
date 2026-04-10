using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CE_Rebate
{
    public class PagoRebate
    {
        public int IdAcuerdo { get; set; }
        public DateTime FechaDocumento { get; set; }
        public decimal MontoTotal { get; set; }
        public string Observacion { get; set; }
        public List<DetallePagoRebate> DetalleEjecuciones { get; set; }

        public List<RetencionVM> Retenciones { get; set; }
          = new List<RetencionVM>();
    }
}
