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
    public class CN_TipoRebate
    {
        private readonly CD_TipoRebate objcd = new CD_TipoRebate();

        public List<TipoRebate> Obtener()
        {
            return objcd.ObtenerTiposRebate();
        }

        public bool Crear(TipoRebate obj, out List<string> mensajes)
        {
            mensajes = new();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras.");

            // Validar estado
            if (obj.oEstado == null || obj.oEstado.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado válido.");

            // Validar duplicado
            if (objcd.ObtenerTiposRebate()
                .Any(r => r.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El tipo de rebate ya existe.");

            if (mensajes.Any())
                return false;

            bool rpta = objcd.Crear(obj, out string msg);
            mensajes.Add(rpta ? "Tipo de rebate creado correctamente." : msg);

            return rpta;
        }

        public bool Editar(TipoRebate obj, out List<string> mensajes)
        {
            mensajes = new();

            // Validar ID
            if (obj.IdTipoRebate == 0)
                mensajes.Add("ID inválido.");

            // Validar nombre
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                mensajes.Add("El nombre es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombre, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("El nombre solo puede contener letras.");

            // Validar estado
            if (obj.oEstado == null || obj.oEstado.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado válido.");

            // Validar duplicado excluyendo el mismo registro
            if (objcd.ObtenerTiposRebate()
                .Any(r => r.Nombre.Equals(obj.Nombre, StringComparison.OrdinalIgnoreCase)
                       && r.IdTipoRebate != obj.IdTipoRebate))
                mensajes.Add("El tipo de rebate ya existe.");

            if (mensajes.Any())
                return false;

            bool rpta = objcd.Editar(obj, out string msg);
            mensajes.Add(rpta ? "Tipo de rebate actualizado correctamente." : msg);

            return rpta;
        }
    }
}
