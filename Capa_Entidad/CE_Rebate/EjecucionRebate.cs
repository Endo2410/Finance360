using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CE_Rebate
{
    public class EjecucionRebate
    {
        public int IdEjecucion { get; set; }
        public int IdAcuerdo { get; set; }

        public string NumeroAcuerdo { get; set; }
        public string NombreAcuerdo { get; set; }
        public string NumeroDocumentoSoporte { get; set; }
        public string UsuarioRegistro { get; set; }

        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }


        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }

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

        public decimal TotalRetenciones { get; set; }

        public decimal TotalNeto { get; set; }

        public decimal MontoPagado { get; set; }

        public string Comentario { get; set; }
        public decimal Ganancia { get; set; }
        public bool Forzado { get; set; }
    }
}
