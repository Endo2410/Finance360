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
    public class CN_ModalidadOperacion
    {
        private readonly CD_ModalidadOperacion objcd = new();

        public List<ModalidadOperacion> ObtenerModalidades()
        {
            return objcd.ObtenerModalidades();
        }

        public bool Crear(ModalidadOperacion obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[A-ZÁÉÍÓÚÑ\s]+$"))
                mensajes.Add("Solo letras mayúsculas (COMPRA / VENTA).");

            if (objcd.ObtenerModalidades()
                .Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("La modalidad ya existe.");

            if (mensajes.Any()) return false;

            bool ok = objcd.Crear(obj, out string msg);
            mensajes.Add(ok ? "Modalidad creada correctamente." : msg);
            return ok;
        }

        public bool Editar(ModalidadOperacion obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (objcd.ObtenerModalidades()
                .Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                && m.IdModalidadOp != obj.IdModalidadOp))
                mensajes.Add("La modalidad ya existe.");

            if (mensajes.Any()) return false;

            bool ok = objcd.Editar(obj, out string msg);
            mensajes.Add(ok ? "Modalidad actualizada correctamente." : msg);
            return ok;
        }
    }
}
