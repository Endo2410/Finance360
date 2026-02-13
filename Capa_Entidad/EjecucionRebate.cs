using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class EjecucionRebate
    {
        public int IdEjecucion { get; set; }
        public int IdAcuerdo { get; set; }

        public string NumeroAcuerdo { get; set; }
        public string NombreAcuerdo { get; set; }
        public string NumeroDocumentoSoporte { get; set; }
        public string UsuarioRegistro { get; set; }


        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string TipoRebate { get; set; }

        public decimal MontoCalculado { get; set; }
        public decimal MontoRebate { get; set; }
        public decimal SaldoPendiente { get; set; }
        public bool CumpleCondicion { get; set; }
        public string ArchivoSoporte { get; set; }

        public int IdEstado { get; set; }
        public string EstadoNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaPago { get; set; }

        public string Criterio { get; set; }
        public decimal ValorCriterio { get; set; }
        public decimal Saldovencido { get; set; }

        public decimal MontoPagado { get; set; }
    }
}
