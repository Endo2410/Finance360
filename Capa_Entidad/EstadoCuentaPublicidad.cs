using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class EstadoCuentaPublicidad
    {
        public int IdEstadoCuenta { get; set; }
        public int IdCampania { get; set; }

        public int IdProveedor { get; set; }
        public string NombreCampania { get; set; }
        public string NumeroCampania { get; set; }
        public string NombreProveedor { get; set; }
        public int NumeroCuota { get; set; }
        public DateTime FechaPagoProgramada { get; set; }
        public decimal MontoCuota { get; set; }
        public string Estado { get; set; }

        // <-- Agregar esta propiedad
        public DateTime FechaInicio { get; set; }
        public decimal MontoInversion { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoPago { get; set; }
    }
    
}
