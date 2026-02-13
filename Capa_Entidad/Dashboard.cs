using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Dashboard
    {
        public string Proveedor { get; set; }
        public decimal Publicidad { get; set; }
        public decimal Rebate { get; set; }
        public decimal Canjes { get; set; }
        public decimal Vencido { get; set; }
        public decimal TotalIngreso { get; set; }


        //saldo pendiente 
        public decimal SaldoPendiente { get; set; }
        public decimal Cobrado { get; set; }
        public decimal PorcentajeRecuperacion { get; set; }
        

        //modulos activos
        public int PublicidadActiva { get; set; }
        public int RebateActivo { get; set; }
        public int VencidoActivo { get; set; }
        public int CanjeActivo { get; set; }
    }
}
