using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Vencido
    {
        public int IdVencido { get; set; }
        public int IdOrdenVencido { get; set; }
        public int HQID { get; set; }
        public string Proveedor { get; set; }
        public string NumeroOrden { get; set; }
        public string StatusOrden { get; set; }
        public string Concepto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Usuario { get; set; }
        public decimal Total { get; set; }
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }


        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }

        public decimal Saldovencido { get; set; }
        public string NombreProveedor { get; set; }
    }
}
