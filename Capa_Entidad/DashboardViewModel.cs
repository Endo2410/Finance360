using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class DashboardViewModel
    {
        public List<Dashboard> TopProveedores { get; set; }
        public List<Dashboard> TopProveedoresSaldoPendiente { get; set; }

        public decimal TotalPublicidad { get; set; }
        public decimal TotalRebate { get; set; }
        public decimal TotalCanjes { get; set; }
        public decimal TotalVencido { get; set; }

        //card para montos vencidos
        public decimal MontoVencidoTotal { get; set; }

        // ESTE ES EL IMPORTANTE
        public Dashboard Resumen { get; set; }


        // NUEVO → Cantidad de activos
        public Dashboard Activos { get; set; }
    }
}
