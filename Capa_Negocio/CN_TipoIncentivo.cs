using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_TipoIncentivo
    {
        private readonly CD_TipoIncentivo objcd = new();

        public List<TipoIncentivo> Obtener()
        {
            return objcd.ObtenerTiposIncentivo();
        }

        public bool Crear(TipoIncentivo obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras.");

            if (objcd.ObtenerTiposIncentivo()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de incentivo ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.Crear(obj, out string msg);
            mensajes.Add(r ? "Tipo de incentivo creado correctamente." : msg);
            return r;
        }

        public bool Editar(TipoIncentivo obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (objcd.ObtenerTiposIncentivo()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                       && t.IdTipoIncentivo != obj.IdTipoIncentivo))
                mensajes.Add("El tipo de incentivo ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.Editar(obj, out string msg);
            mensajes.Add(r ? "Tipo de incentivo actualizado correctamente." : msg);
            return r;
        }
    }
}
