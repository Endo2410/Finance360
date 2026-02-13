using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
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
    }
}
