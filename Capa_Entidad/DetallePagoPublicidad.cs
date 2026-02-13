using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class DetallePagoPublicidad
    {
       
        public int IdEstadoCuenta { get; set; }
        public decimal MontoPagado { get; set; }
        public int IdTipoDocumento { get; set; } // Tipo de pago por cuota
        public string RutaComprobante { get; set; }
        public string usuarioPago { get; set; }
        public string NumeroConfirmacion { get; set; }
    }

}
