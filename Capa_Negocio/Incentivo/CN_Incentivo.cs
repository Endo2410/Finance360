using Capa_Dato;
using Capa_Entidad.CE_Incentivo;

namespace Capa_Negocio.Incentivo
{
    public class CN_Incentivo
    {
        private readonly CD_Incentivo cd = new CD_Incentivo();

        public List<Capa_Entidad.CE_Incentivo.Incentivo> Obtener() => cd.Obtener();

        public bool Crear(Capa_Entidad.CE_Incentivo.Incentivo obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdSucursal <= 0)
                mensajes.Add("Debe seleccionar una sucursal.");

            if (obj.IdProveedor <= 0)
                mensajes.Add("Debe seleccionar un proveedor.");

            if (string.IsNullOrEmpty(obj.DocumentoAdjunto))
                mensajes.Add("Debe adjuntar un documento.");

            if (mensajes.Any()) return false;

            bool ok = cd.Crear(obj, out string msg);
            if (!ok) mensajes.Add(msg);

            return ok;
        }

        public bool Editar(Capa_Entidad.CE_Incentivo.Incentivo obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdIncentivo <= 0)
                mensajes.Add("Incentivo inválido.");

            if (mensajes.Any()) return false;

            bool ok = cd.Editar(obj, out string msg);
            if (!ok) mensajes.Add(msg);

            return ok;
        }

        public bool RegistrarPago(PagoIncentivo pago, out List<string> mensajes, out string numeroDocumento)
        {
            mensajes = new List<string>();

            numeroDocumento = "";

            if (pago.Detalles == null ||
                !pago.Detalles.Any())
            {
                mensajes.Add(
                    "Debe ingresar al menos un pago.");
            }

            if (pago.Detalles.Any(x =>
                x.MontoPagado <= 0))
            {
                mensajes.Add(
                    "El monto debe ser mayor a cero.");
            }

            if (mensajes.Any())
                return false;


            bool ok = cd.RegistrarPago(
                pago,
                out string msg,
                out string num);

            if (!ok)
                mensajes.Add(msg);

            numeroDocumento = num;

            return ok;
        }

        public List<DetallePagoIncentivo> ObtenerDetallePago(int idIncentivo)
        {
            return cd.ObtenerDetallePago(idIncentivo);
        }
    }
}
