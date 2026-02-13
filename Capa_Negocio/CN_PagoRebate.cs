using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_PagoRebate
    {
        private readonly CD_PagoRebate objcd = new();

        public bool RegistrarPago(PagoRebate pago, out List<string> mensajes, out string numeroDocumento)
        {
            mensajes = new();
            numeroDocumento = "";

            if (pago.DetalleEjecuciones == null || !pago.DetalleEjecuciones.Any())
                mensajes.Add("Debe seleccionar al menos una ejecución.");

            if (pago.DetalleEjecuciones.Any(x => x.MontoPagado <= 0))
                mensajes.Add("El monto pagado debe ser mayor a cero.");

            if (mensajes.Any()) return false;

            bool ok = objcd.RegistrarPago(pago, out string msg, out string num);
            if (!ok) mensajes.Add(msg);

            numeroDocumento = num;
            return ok;
        }
    }
}
