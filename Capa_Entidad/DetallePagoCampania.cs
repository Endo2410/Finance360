using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class DetallePagoCampania
    {
        public int? IdDetallePago { get; set; }
        public string DocumentoPago { get; set; }
        public decimal? MontoPagado { get; set; }
        public string TipoDocumento { get; set; }
        public DateTime? FechaDocumento { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Comprobante { get; set; }
        public string usuarioPago { get; set; }
        public string NumeroConfirmacion { get; set; }
        public int? IdEstado { get; set; }
        public bool NotaCreditoAplicada { get; set; }
        public string Observacion { get; set; }

    }
}
