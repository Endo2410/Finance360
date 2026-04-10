using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_TipoRetencion
    {
        private readonly CD_TipoRetencion objcd = new();

        public List<TipoRetencion> Obtener()
        {
            return objcd.Obtener();
        }

        public bool Crear(TipoRetencion obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (obj.Porcentaje <= 0)
                mensajes.Add("El porcentaje debe ser mayor a 0.");

            if (objcd.Obtener().Any(x => x.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de retención ya existe.");

            if (mensajes.Any()) return false;

            bool ok = objcd.Crear(obj, out string msg);
            mensajes.Add(ok ? "Tipo de retención creado correctamente." : msg);
            return ok;
        }

        public bool Editar(TipoRetencion obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (obj.Porcentaje <= 0)
                mensajes.Add("El porcentaje debe ser mayor a 0.");

            if (objcd.Obtener().Any(x =>
                x.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                && x.IdTipoRetencion != obj.IdTipoRetencion))
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
