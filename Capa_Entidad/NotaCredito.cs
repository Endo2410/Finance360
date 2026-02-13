using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class NotaCredito
    {
        public int IdNC { get; set; }
        public string NumeroNC { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Monto { get; set; }
        public string TipoOrigen { get; set; }
        public int IdOrigen { get; set; }
        public DateTime FechaRegistro { get; set; }

        public int IdProveedor { get; set; }
        public int IdEstado { get; set; }

        public Proveedor oProveedor { get; set; }
        public Estado oEstado { get; set; }

        public string NumeroDocumentoOrigen { get; set; }  
        public string NumeroDocumentoConfirmacion { get; set; }  
        public string DocumentoAdjunto { get; set; }

        // ✅ Nueva propiedad para observación del usuario
        public string Observacion { get; set; }

        public List<DetallePagoNC> DetallePagos { get; set; }
    }

    public class DetallePagoNC
    {
        public string NumeroConfirmacion { get; set; }
        public string RutaComprobante { get; set; }
        public string Observacion { get; set; }
        public string UsuarioPago { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string Comprobante { get; set; }
        public string UsuarioAplica { get; set; }

    }
}
