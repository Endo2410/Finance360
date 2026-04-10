using Capa_Dato.Cuentas_Pagar;
using Capa_Entidad.Cuentas_Por_Pagar;

namespace Capa_Negocio.Cuentas_Por_Pagar
{
    public class CN_CondicionPagoProveedor
    {
        private CD_CondicionPagoProveedor cd = new();

        public List<CondicionPagoProveedor> Obtener()
        {
            return cd.Obtener();
        }

        public bool Crear(CondicionPagoProveedor obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdProveedor <= 0)
                mensajes.Add("Debe seleccionar proveedor");

            if (obj.DiasCredito <= 0)
                mensajes.Add("Debe ingresar días de crédito");

            if (mensajes.Any())
                return false;

            bool ok = cd.Crear(obj, out string msg);

            if (!ok)
                mensajes.Add(msg);

            return ok;
        }

        public bool Editar(CondicionPagoProveedor obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdCondicion <= 0)
                mensajes.Add("Condición inválida.");

            if (obj.IdProveedor <= 0)
                mensajes.Add("Debe seleccionar proveedor.");

            if (obj.DiasCredito <= 0)
                mensajes.Add("Debe ingresar días de crédito.");

            if (mensajes.Any())
                return false;

            bool ok = cd.Editar(obj, out string msg);

            if (!ok)
                mensajes.Add(msg);

            return ok;
        }
    }
}