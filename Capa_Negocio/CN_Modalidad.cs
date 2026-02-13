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
    public class CN_Modalidad
    {
        private readonly CD_Modalidad objcd = new CD_Modalidad();

        public List<Modalidad> ObtenerModalidades()
        {
            return objcd.ObtenerModalidades();
        }

        public bool CrearModalidad(Modalidad obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (string.IsNullOrWhiteSpace(obj.TipoIntervalo))
                mensajes.Add("El tipo de intervalo es obligatorio.");

            if (obj.ValorIntervalo <= 0)
                mensajes.Add("El valor del intervalo debe ser mayor a cero.");

            // Validaciones lógicas
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre de la modalidad es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre de la modalidad solo puede contener letras.");

            // Verificar duplicados
            var lista = objcd.ObtenerModalidades();
            if (lista.Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("La modalidad ya existe.");

            if (mensajes.Any())
                return false;

            bool resultado = objcd.CrearModalidad(obj, out string msg);
            if (!resultado) mensajes.Add(msg);
            else mensajes.Add("Modalidad creada correctamente.");

            return resultado;
        }

        public bool EditarModalidad(Modalidad obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (string.IsNullOrWhiteSpace(obj.TipoIntervalo))
                mensajes.Add("El tipo de intervalo es obligatorio.");

            if (obj.ValorIntervalo <= 0)
                mensajes.Add("El valor del intervalo debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre de la modalidad es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre de la modalidad solo puede contener letras.");

            var lista = objcd.ObtenerModalidades();
            if (lista.Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase) && m.IdModalidad != obj.IdModalidad))
                mensajes.Add("La modalidad ya existe.");

            if (mensajes.Any())
                return false;

            bool resultado = objcd.EditarModalidad(obj, out string msg);
            if (!resultado) mensajes.Add(msg);
            else mensajes.Add("Modalidad actualizada correctamente.");

            return resultado;
        }
    }
}
