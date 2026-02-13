using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class PagoVencido
    {
        public int IdVencido { get; set; }
        public DateTime FechaDocumento { get; set; }
        public decimal MontoTotal { get; set; }
        public string Observacion { get; set; }
        public List<DetallePagoVencido> DetalleCuotas { get; set; }
    }
}

