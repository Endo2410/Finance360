using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class DetallePagoRebate
    {
        public int IdEjecucionRebate { get; set; }
        public decimal MontoPagado { get; set; }
        public int IdTipoDocumento { get; set; }
        public string RutaComprobante { get; set; }
        public string usuarioPago { get; set; }
        public string NumeroConfirmacion { get; set; }
    }

}
