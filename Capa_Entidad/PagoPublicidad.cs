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
        public decimal MontoTotal { get; set; }
        public string Observacion { get; set; }


        public List<DetallePagoPublicidad> DetalleCuotas { get; set; }
            = new List<DetallePagoPublicidad>();

    }
}
