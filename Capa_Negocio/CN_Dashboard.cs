using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_Dashboard
    {
        private readonly CD_Dashboard cd = new CD_Dashboard();

        public List<Dashboard> TopProveedores(DateTime? fechaInicio, DateTime? fechaFin, int? idProveedor)
        {
            return cd.ObtenerTopProveedores(fechaInicio, fechaFin, idProveedor);
        }



        public List<Dashboard> TopProveedoresSaldoPendiente(int? idProveedor)
        {
            return cd.ObtenerTopProveedoresSaldoPendiente(idProveedor);
        }


        public DashboardViewModel ObtenerDashboard(DateTime? fechaInicio, DateTime? fechaFin, int? idProveedor)
        {
            var proveedores = cd.ObtenerTopProveedores(fechaInicio, fechaFin, idProveedor);
            var resumen = cd.ObtenerResumenFinanciero(fechaInicio, fechaFin, idProveedor);

            var pendientes = cd.ObtenerTopProveedoresSaldoPendiente(idProveedor);
            var activos = cd.ObtenerCantidadActivos();


            return new DashboardViewModel
            {
                TopProveedores = proveedores,
                TopProveedoresSaldoPendiente = pendientes,

                TotalPublicidad = proveedores.Sum(x => x.Publicidad),
                TotalRebate = proveedores.Sum(x => x.Rebate),
                TotalCanjes = proveedores.Sum(x => x.Canjes),
                TotalVencido = proveedores.Sum(x => x.Vencido),


                //card monto vencido 
                MontoVencidoTotal = pendientes.Sum(x => x.SaldoPendiente),



                //demas card 
                Resumen = resumen,

                  //modulos activos
                  Activos = activos
            };
        }

        public Dashboard ObtenerResumenFinanciero(DateTime? fechaInicio, DateTime? fechaFin, int? idProveedor)
        {
            return cd.ObtenerResumenFinanciero(fechaInicio, fechaFin, idProveedor);
        }

    }
}
