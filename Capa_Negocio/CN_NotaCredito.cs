using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_NotaCredito
    {
        private readonly CD_NotaCredito objcd = new CD_NotaCredito();

        public List<NotaCredito> ListarNotasCredito()
        {
            return objcd.ListarNotasCredito();
        }

        public List<DetallePagoNC> ObtenerDetallePago(int idNC)
        {
            return objcd.ObtenerDetallePago(idNC);
        }
    }
}
