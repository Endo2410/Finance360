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
    public class CN_Moneda
    {
        private readonly CD_Moneda objcd = new CD_Moneda();

        public List<Moneda> ObtenerMonedas()
        {
            return objcd.ObtenerMonedas();
        }

        public bool CrearMoneda(Moneda obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones lógicas
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre de la moneda es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre de la moneda solo puede contener letras.");

            if (string.IsNullOrWhiteSpace(obj.Simbolo))
                mensajes.Add("El símbolo de la moneda es obligatorio.");

            // Verificar duplicados
            var listaMonedas = objcd.ObtenerMonedas();
            if (listaMonedas.Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("La moneda ya existe.");

            if (mensajes.Any())
                return false;

            // Insertar en BD
            bool resultado = objcd.CrearMoneda(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);
            else
                mensajes.Add("Moneda creada correctamente.");

            return resultado;
        }

        public bool EditarMoneda(Moneda obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones lógicas
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre de la moneda es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre de la moneda solo puede contener letras.");

            if (string.IsNullOrWhiteSpace(obj.Simbolo))
                mensajes.Add("El símbolo de la moneda es obligatorio.");

            // Verificar duplicados ignorando el registro actual
            var listaMonedas = objcd.ObtenerMonedas();
            if (listaMonedas.Any(m => m.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase) && m.IdMoneda != obj.IdMoneda))
                mensajes.Add("La moneda ya existe.");

            if (mensajes.Any())
                return false;

            // Editar en BD
            bool resultado = objcd.EditarMoneda(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);
            else
                mensajes.Add("Moneda actualizada correctamente.");

            return resultado;
        }
    }
}
