using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class PagoCanje
    {
        public int IdCanje { get; set; }
        public decimal MontoTotal { get; set; }
        public string Observacion { get; set; }
        public List<DetallePagoCanje> DetalleCanjes { get; set; }
         public List<RetencionVM> Retenciones { get; set; }
            = new List<RetencionVM>();
    }

    public class DetallePagoCanje
    {
        public int IdCanjeDetalle { get; set; } // Puede ser IdEjecucion equivalente
        public decimal MontoPagado { get; set; }

        public int IdTipoDocumento { get; set; }
        public string NumeroConfirmacion { get; set; }

        public string RutaComprobante { get; set; }
        public string UsuarioPago { get; set; }

    }

    public class DetallePagoCanje1
    {
        public int IdDetallePago { get; set; }        // Coincide con SP -> ID_DETALLE_PAGO
        public decimal MontoPagado { get; set; }
        public string TipoDocumento { get; set; }    // Nombre del tipo de documento
        public string DocumentoPago { get; set; }    // Número del documento
        public string NumeroConfirmacion { get; set; }

        public string Comprobante { get; set; }  // Comprobante
        public string Observacion { get; set; }  // Comprobante
        public string UsuarioPago { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool NotaCreditoAplicada { get; set; }
        public int? IdEstado { get; set; }

    }

}
