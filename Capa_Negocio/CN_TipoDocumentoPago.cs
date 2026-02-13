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
    public class CN_TipoDocumentoPago
    {
        private readonly CD_TipoDocumentoPago objcd = new CD_TipoDocumentoPago();

        // Obtener todos los tipos
        public List<TipoDocumentoPago> ObtenerTipos()
        {
            return objcd.ObtenerTipos();
        }

        // Crear tipo de documento
        public bool CrearTipo(TipoDocumentoPago obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre del tipo de documento es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras y espacios.");

            // Verificar duplicados
            var lista = objcd.ObtenerTipos();
            if (lista.Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de documento ya existe.");

            if (mensajes.Any())
                return false;

            // Insertar en BD
            bool resultado = objcd.CrearTipo(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);
            else
                mensajes.Add("Tipo de documento creado correctamente.");

            return resultado;
        }

        // Editar tipo de documento
        public bool EditarTipo(TipoDocumentoPago obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre del tipo de documento es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras y espacios.");

            // Verificar duplicados ignorando el registro actual
            var lista = objcd.ObtenerTipos();
            if (lista.Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                               && m.IdTipoDoc != obj.IdTipoDoc))
                mensajes.Add("El tipo de documento ya existe.");

            if (mensajes.Any())
                return false;

            // Actualizar en BD
            bool resultado = objcd.EditarTipo(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);
            else
                mensajes.Add("Tipo de documento actualizado correctamente.");

            return resultado;
        }
    }

}
