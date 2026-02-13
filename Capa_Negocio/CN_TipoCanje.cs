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
    public class CN_TipoCanje
    {
        private readonly CD_TipoCanje objcd = new();

        public List<TipoCanje> Obtener()
        {
            return objcd.Obtener();
        }

        public bool Crear(TipoCanje obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras.");

            if (obj.oEstado == null || obj.oEstado.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado válido.");

            if (objcd.Obtener()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de canje ya existe.");

            if (mensajes.Any())
                return false;

            bool rpta = objcd.Crear(obj, out string msg);
            mensajes.Add(rpta ? "Tipo de canje creado correctamente." : msg);
            return rpta;
        }

        public bool Editar(TipoCanje obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdTipoCanje == 0)
                mensajes.Add("ID inválido.");

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");

            if (obj.oEstado == null || obj.oEstado.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado válido.");

            if (objcd.Obtener()
                .Any(t => t.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                       && t.IdTipoCanje != obj.IdTipoCanje))
                mensajes.Add("El tipo de canje ya existe.");

            if (mensajes.Any())
                return false;

            bool rpta = objcd.Editar(obj, out string msg);
            mensajes.Add(rpta ? "Tipo de canje actualizado correctamente." : msg);
            return rpta;
        }
    }
}
