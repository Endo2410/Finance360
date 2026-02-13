using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_OrdenCompra
    {
        private CD_OrdenCompra objcd = new CD_OrdenCompra();

        public List<OrdenCompra> ObtenerReporteOrdenes(DateTime inicio, DateTime fin)
        {
            return objcd.ObtenerReporteOrdenes(inicio, fin);
        }
        public List<OrdenCompra> ObtenerOrdenes(DateTime inicio, DateTime fin)
        {
            return objcd.ObtenerOrdenes(inicio, fin);
        }

        public int InsertarOrdenesNuevas()
        {
            return objcd.InsertarOrdenesNuevas();
        }
    }
}
