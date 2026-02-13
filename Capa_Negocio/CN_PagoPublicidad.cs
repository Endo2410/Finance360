using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Capa_Dato.CD_PagoPublicidad;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio
{
    public class CN_PagoPublicidad
    {
        private readonly CD_PagoPublicidad objcd = new CD_PagoPublicidad();

        public bool RegistrarPago(PagoPublicidad pago, out List<string> mensajes, out string numeroDocumento)
        {
            mensajes = new List<string>();
            numeroDocumento = "";

            if (pago.DetalleCuotas == null || !pago.DetalleCuotas.Any())
                mensajes.Add("Debe seleccionar al menos una cuota.");

            if (pago.DetalleCuotas.Any(x => x.MontoPagado <= 0))
                mensajes.Add("El monto pagado no puede ser cero.");

            if (pago.DetalleCuotas.Any(x => x.IdTipoDocumento <= 0))
                mensajes.Add("Cada cuota debe tener un tipo de pago.");

            if (mensajes.Any()) return false;

            bool resultado = objcd.RegistrarPago(pago, out string msg, out string numDoc);
            if (!resultado) mensajes.Add(msg);

            numeroDocumento = numDoc;
            return resultado;
        }
    }
}
