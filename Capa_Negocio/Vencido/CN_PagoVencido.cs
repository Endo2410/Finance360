using Capa_Dato.Vencido;
using Capa_Entidad;


namespace Capa_Negocio
{
    public class CN_PagoVencido
    {
        private readonly CD_PagoVencido objcd = new CD_PagoVencido();

        public bool RegistrarPago(PagoVencido pago, out List<string> mensajes, out string numeroDocumento)
        {
            mensajes = new List<string>();
            numeroDocumento = "";

            if (pago.DetalleCuotas == null || !pago.DetalleCuotas.Any())
                mensajes.Add("Debe seleccionar al menos un vencido.");

            if (pago.DetalleCuotas.Any(x => x.MontoPagado <= 0))
                mensajes.Add("El monto pagado no puede ser cero.");

            if (pago.DetalleCuotas.Any(x => x.IdTipoDocumento <= 0))
                mensajes.Add("Cada pago debe tener un tipo de documento.");

            if (mensajes.Any()) return false;

            bool resultado = objcd.RegistrarPago(pago, out string msg, out string numDoc);
            if (!resultado) mensajes.Add(msg);

            numeroDocumento = numDoc;
            return resultado;
        }
    }
}
