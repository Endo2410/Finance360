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
    public class CN_TipoPublicidad
    {
        private readonly CD_TipoPublicidad objcd = new CD_TipoPublicidad();

        public List<TipoPublicidad> Obtener()
        {
            return objcd.ObtenerTiposPublicidad();
        }

        public bool Crear(TipoPublicidad obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras.");

            if (objcd.ObtenerTiposPublicidad()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de publicidad ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.Crear(obj, out string msg);
            mensajes.Add(r ? "Tipo de publicidad creado correctamente." : msg);
            return r;
        }

        public bool Editar(TipoPublicidad obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (objcd.ObtenerTiposPublicidad()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                       && t.IdTipoPublicidad != obj.IdTipoPublicidad))
                mensajes.Add("El tipo de publicidad ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.Editar(obj, out string msg);
            mensajes.Add(r ? "Tipo de publicidad actualizado correctamente." : msg);
            return r;
        }
    }
}
