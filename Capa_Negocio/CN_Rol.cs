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
    public class CN_Rol
    {
        private readonly CD_Rol objcd = new();

        public List<Rol> Obtener()
        {
            return objcd.Obtener();
        }

        public bool Crear(Rol obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre del rol es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
                mensajes.Add("La descripción es obligatoria.");

            if (obj.oEstado == null || obj.oEstado.IdEstado <= 0)
                mensajes.Add("Debe seleccionar un estado válido.");

            if (objcd.Obtener()
                .Any(r => r.Nombre.ToLower() == obj.Nombre.ToLower()))
                mensajes.Add("El rol ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.Crear(obj, out string msg);
            mensajes.Add(r ? "Rol creado correctamente." : msg);
            return r;
        }

        public bool Editar(Rol obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdRol <= 0)
                mensajes.Add("Rol inválido.");

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre del rol es obligatorio.");

            if (objcd.Obtener()
                .Any(r => r.Nombre.ToLower() == obj.Nombre.ToLower()
                       && r.IdRol != obj.IdRol))
                mensajes.Add("El rol ya existe.");

            if (mensajes.Any()) return false;

            bool r = objcd.Editar(obj, out string msg);
            mensajes.Add(r ? "Rol actualizado correctamente." : msg);
            return r;
        }
    }
}
