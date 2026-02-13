using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_Vencidos
    {
        private CD_Vencidos objcd = new CD_Vencidos();

        public List<Vencido> ObtenerVencidos(DateTime inicio, DateTime fin)
        {
            return objcd.ObtenerVencidos(inicio, fin);
        }

        public List<Vencido> ObtenerVencidos()
        {
            return objcd.ObtenerVencidosPendientes();
        }

        public int InsertarVencidosNuevos()
        {
            return objcd.InsertarVencidosNuevos();
        }

        public List<Vencido> ObtenerVencido1()
        {
            return objcd.ObtenerVencido1();
        }

        public List<DetallePagoVencido> ObtenerDetallePagoVencido(int idVencido)
        {
            return objcd.ObtenerDetallePagoVencido(idVencido);
        }

        public bool AnularPagoVencido(int idDetallePago, string usuario)
        {
            if (idDetallePago <= 0)
                return false;

            return objcd.AnularPagoVencido(idDetallePago, usuario);
        }

    }
}
