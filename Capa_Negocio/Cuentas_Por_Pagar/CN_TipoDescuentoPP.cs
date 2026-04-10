using Capa_Dato.Cuentas_Por_Pagar;
using Capa_Entidad.Cuentas_Por_Pagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Cuentas_Por_Pagar
{
    public class CN_TipoDescuentoPP
    {
        private readonly CD_TipoDescuentoPP objcd = new();

        public List<TipoDescuentoPP> Obtener()
        {
            return objcd.Obtener();
        }

        public bool Crear(TipoDescuentoPP obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (obj.Porcentaje <= 0)
                mensajes.Add("El porcentaje debe ser mayor a 0.");

            if (objcd.Obtener().Any(x =>
                x.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de descuento ya existe.");

            if (mensajes.Any()) return false;

            bool ok = objcd.Crear(obj, out string msg);

            mensajes.Add(ok ? "Tipo de descuento creado correctamente." : msg);

            return ok;
        }

        public bool Editar(TipoDescuentoPP obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (obj.Porcentaje <= 0)
                mensajes.Add("El porcentaje debe ser mayor a 0.");

            if (objcd.Obtener().Any(x =>
                x.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                && x.IdTipoDescuento != obj.IdTipoDescuento))
            {
                mensajes.Add("El nombre ya existe.");
            }

            if (mensajes.Any()) return false;

            bool ok = objcd.Editar(obj, out string msg);

            mensajes.Add(ok ? "Actualizado correctamente." : msg);

            return ok;
        }
    }
}
