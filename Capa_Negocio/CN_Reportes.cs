using Capa_Dato;
using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_Reportes
    {
        private CD_Reportes objcd = new CD_Reportes();

        public List<ComprasCliente> ObtenerComprasCliente(string accountNumber, DateTime inicio, DateTime fin)
        {
            return objcd.ObtenerComprasCliente(accountNumber, inicio, fin);
        }

        public List<ReporteCompras> ObtenerReporte(DateTime inicio, DateTime fin, string proveedor, string laboratorio)
        {
            return objcd.ObtenerReporte(inicio, fin, proveedor, laboratorio);
        }

        public List<OrdenSinRecibir> ObtenerOrdenessinrecibir(DateTime inicio, DateTime fin)
        {
            return objcd.ObtenerOrdenessinrecibir(inicio, fin);
        }
    }
}
