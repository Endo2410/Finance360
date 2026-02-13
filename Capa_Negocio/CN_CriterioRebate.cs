using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{

    public class CN_CriterioRebate
    {
        private readonly CD_CriterioRebate objcd = new();

        public List<CriterioRebate> ObtenerCriterios()
        {
            return objcd.ObtenerCriterios();
        }

        public bool Crear(CriterioRebate obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Operador))
                mensajes.Add("El operador es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
                mensajes.Add("La descripción es obligatoria.");

            if (objcd.ObtenerCriterios()
                .Any(c => c.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El criterio ya existe.");

            if (mensajes.Any()) return false;

            bool ok = objcd.Crear(obj, out string msg);
            mensajes.Add(ok ? "Criterio creado correctamente." : msg);
            return ok;
        }

        public bool Editar(CriterioRebate obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Operador))
                mensajes.Add("El operador es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
                mensajes.Add("La descripción es obligatoria.");

            if (objcd.ObtenerCriterios()
                .Any(c => c.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                          && c.IdCriterio != obj.IdCriterio))
                mensajes.Add("El criterio ya existe.");

            if (mensajes.Any()) return false;

            bool ok = objcd.Editar(obj, out string msg);
            mensajes.Add(ok ? "Criterio actualizado correctamente." : msg);
            return ok;
        }
    }
}
