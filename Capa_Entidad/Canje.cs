using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Canje
    {
        public int IdCanje { get; set; }
        public string NumeroCanje { get; set; }
        public string NombreCanje { get; set; }

        public decimal Saldovencido { get; set; }
        public int IdProveedor { get; set; }
        public Proveedor oProveedor { get; set; }

        public int IdTipoCanje { get; set; }
        public TipoCanje oTipoCanje { get; set; }

        public decimal Volumen { get; set; }
        public decimal Monto { get; set; }

        public string UsuarioRegistro { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string Comentario { get; set; }

        public Estado oEstado { get; set; }
        public int IdEstado { get; set; }
        public int? IdDepartamento { get; set; }
        public Departamento ODepartamento { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string NombreProveedor { get; set; }

        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }

    }
}
