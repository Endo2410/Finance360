using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Capa_Negocio.Incentivo
{
    public class CN_Tipousoincentivo
    {
        private readonly CD_TipoUsoIncentivo objcd = new CD_TipoUsoIncentivo();

        public List<TipoUsoIncentivo> ObtenerUso()
        {
            return objcd.ObtenerTiposUsoIncentivo();
        }

        public bool CrearUso(TipoUsoIncentivo obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras.");

            if (objcd.ObtenerTiposUsoIncentivo()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de uso incentivo  ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.CrearUso(obj, out string msg);
            mensajes.Add(r ? "Tipo de uso incentivo creado correctamente." : msg);
            return r;
        }

        public bool EditarUso(TipoUsoIncentivo obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (objcd.ObtenerTiposUsoIncentivo()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                       && t.IdTipoUsoIncentivo != obj.IdTipoUsoIncentivo))
                mensajes.Add("El tipo de uso incentivo ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.EditarUso(obj, out string msg);
            mensajes.Add(r ? "Tipo de uso incentivo actualizado correctamente." : msg);
            return r;
        }
    }
}
