using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio
{
    public class CN_Estado
    {
        private readonly CD_Estado objcd = new CD_Estado();

        public List<Estado> ObtenerEstado(string modulo, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                // Filtra solo los estados que pertenecen al módulo
                var lista = objcd.ObtenerEstados().Where(e => e.Modulo == modulo).ToList();
                return lista;
            }
            catch (Exception ex)
            {
                mensaje = "Error al obtener estados: " + ex.Message;
                return new List<Estado>();
            }
        }

        public List<Estado> ObtenerEstados() => objcd.ObtenerEstados();

        public bool CrearEstado(Estado obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre del estado es obligatorio.");
            else if (obj.Nombre.Length > 50)
                mensajes.Add("El nombre no puede superar 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 200)
                mensajes.Add("La descripción no puede superar 200 caracteres.");

            if (mensajes.Any()) return false;

            bool resultado = objcd.CrearEstado(obj, out string msg);
            if (!resultado) mensajes.Add(msg);

            return resultado;
        }

        public bool EditarEstado(Estado obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre del estado es obligatorio.");
            else if (obj.Nombre.Length > 50)
                mensajes.Add("El nombre no puede superar 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 200)
                mensajes.Add("La descripción no puede superar 200 caracteres.");

            if (mensajes.Any()) return false;

            bool resultado = objcd.EditarEstado(obj, out string msg);
            if (!resultado) mensajes.Add(msg);

            return resultado;
        }
    }
}
