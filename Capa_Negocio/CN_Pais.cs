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
    public class CN_Pais
    {
        private readonly CD_Pais objcd = new CD_Pais();

        public List<Pais> ObtenerPaises()
        {
            return objcd.ObtenerPaises();
        }

        public bool CrearPais(Pais obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El campo Nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre del país solo puede contener letras.");

            // Verificar duplicados
            var listaPaises = objcd.ObtenerPaises();
            if (listaPaises.Any(p => p.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El país ya existe.");

            if (mensajes.Any())
                return false;

            // Insertar en BD
            bool resultado = objcd.CrearPais(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);
            else
                mensajes.Add("País creado correctamente.");

            return resultado;
        }

        public bool EditarPais(Pais obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El campo Nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre del país solo puede contener letras.");

            // Verificar duplicados ignorando el país actual
            var listaPaises = objcd.ObtenerPaises();
            if (listaPaises.Any(p => p.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase) && p.IdPais != obj.IdPais))
                mensajes.Add("El país ya existe.");

            if (mensajes.Any())
                return false;

            // Editar en BD
            bool resultado = objcd.EditarPais(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);
            else
                mensajes.Add("País actualizado correctamente.");

            return resultado;
        }
    }
}
