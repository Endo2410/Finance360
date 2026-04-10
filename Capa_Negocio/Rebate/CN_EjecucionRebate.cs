using Capa_Dato.Rebate;
using Capa_Entidad;
using Capa_Entidad.CE_Rebate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Rebate
{
    public class CN_EjecucionRebate
    {
        private readonly CD_EjecucionRebate objcd = new CD_EjecucionRebate();

        public List<EjecucionRebate> ObtenerEjecuciones()
        {
            return objcd.ObtenerEjecuciones();
        }

        public bool CrearEjecucion(EjecucionRebate obj, out string mensaje)
        {
            return objcd.CrearEjecucion(obj, out mensaje);
        }

        public EjecucionRebate ObtenerEjecucionPorId(int id)
        {
            return objcd.ObtenerEjecuciones().FirstOrDefault(e => e.IdEjecucion == id);
        }

        public bool EditarEjecucion(EjecucionRebate obj, out string mensaje)
        {
            return objcd.EditarEjecucion(obj, out mensaje);
        }

        public List<EjecucionRebate> ObtenerEjecucionesRebateResumen()
        {
            return objcd.ObtenerEjecucionesRebateResumen();
        }

        public List<DetallePagoEjecucionRebate> ObtenerDetallePagoEjecucionRebate(int idEjecucion)
        {
            return objcd.ObtenerDetallePagoEjecucionRebate(idEjecucion);
        }

        public bool AnularPagoEjecucionRebate(int idDetallePago, string usuario)
        {
            if (idDetallePago <= 0)
                return false;

            return objcd.AnularPagoEjecucionRebate(idDetallePago, usuario);
        }

        //ITEM

        public List<EjecucionRebate> ObtenerEjecucionesItem()
        {
            return objcd.ObtenerEjecucionesItem();
        }

        public bool CrearEjecucionItemCompleto(EjecucionRebate obj, string detallesJson, out int idEjecucion, out string mensaje)
        {
            return objcd.CrearEjecucionItemCompleto(obj, detallesJson, out idEjecucion, out mensaje);
        }

        public bool EditarEjecucionItem(EjecucionRebate obj, string detallesJson, out string mensaje)
        {
            return objcd.EditarEjecucionItem(obj, detallesJson, out mensaje);
        }

        public List<DetalleAcuerdo> ObtenerDetalleEjecucion(int idEjecucion)
        {
            return objcd.ObtenerDetalleEjecucion(idEjecucion);
        }

        public List<EjecucionRebate> ObtenerEjecucionesDescuento()
        {
            return objcd.ObtenerEjecucionesDescuento();
        }

        public List<DetalleAcuerdo> ObtenerDetalleEjecucionDescuento(int idEjecucion)
        {
            return objcd.ObtenerDetalleEjecucionDescuento(idEjecucion);
        }

    }
}
