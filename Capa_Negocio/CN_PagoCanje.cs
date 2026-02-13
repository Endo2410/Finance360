using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_PagoCanje
    {
        private readonly CD_PagoCanje objcd = new();

        public bool RegistrarPago(PagoCanje pago, out List<string> mensajes, out string numeroDocumento)
        {
            mensajes = new();
            numeroDocumento = "";

            if (pago.DetalleCanjes == null || !pago.DetalleCanjes.Any())
                mensajes.Add("Debe seleccionar al menos un canje.");

            if (pago.DetalleCanjes.Any(x => x.MontoPagado <= 0))
                mensajes.Add("El monto pagado debe ser mayor a cero.");

            if (mensajes.Any()) return false;

            bool ok = objcd.RegistrarPago(pago, out string msg, out string num);
            if (!ok) mensajes.Add(msg);

            numeroDocumento = num;
            return ok;
        }
    }
}
