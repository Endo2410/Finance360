using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_AcuerdoRebate
    {
        private readonly CD_AcuerdoRebate objcd = new CD_AcuerdoRebate();

        public List<AcuerdoRebate> ObtenerAcuerdos()
        {
            return objcd.ObtenerAcuerdos();
        }

        public bool CrearAcuerdo(AcuerdoRebate obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Nombre obligatorio
            if (string.IsNullOrWhiteSpace(obj.NombreAcuerdo))
                mensajes.Add("El nombre del acuerdo es obligatorio.");

            // ValorCriterio y Ganancia
            if (obj.ValorCriterio <= 0)
                mensajes.Add("El valor del criterio debe ser mayor a cero.");

            if (obj.Ganancia <= 0)
                mensajes.Add("La ganancia debe ser mayor a cero.");

            // Fechas obligatorias y consistentes
            if (obj.FechaInicio == default)
                mensajes.Add("Debe seleccionar la Fecha Inicio.");

            if (obj.FechaFin == default)
                mensajes.Add("Debe seleccionar la Fecha Fin.");

            if (obj.FechaInicio > obj.FechaFin)
                mensajes.Add("La fecha de inicio no puede ser mayor que la fecha de fin.");

            // Proveedor, modalidad, tipo rebate, criterio, país, moneda
            if (obj.IdProveedor <= 0)
                mensajes.Add("Debe seleccionar un proveedor.");

            if (obj.IdModalidadOp <= 0)
                mensajes.Add("Debe seleccionar una modalidad de operación.");

            if (obj.IdTipoRebate <= 0)
                mensajes.Add("Debe seleccionar un tipo de rebate.");

            if (obj.IdCriterio <= 0)
                mensajes.Add("Debe seleccionar un criterio.");

            if (obj.IdPais <= 0)
                mensajes.Add("Debe seleccionar un país.");

            if (obj.IdMoneda <= 0)
                mensajes.Add("Debe seleccionar una moneda.");

            // Documento obligatorio
            if (string.IsNullOrWhiteSpace(obj.Documento))
                mensajes.Add("Debe adjuntar un documento para el acuerdo.");
            else
            {
                // Validar extensión del documento
                string ext = System.IO.Path.GetExtension(obj.Documento).ToLower();
                var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif" };
                if (!extensionesPermitidas.Contains(ext))
                    mensajes.Add("El documento debe ser PDF o imagen (jpg, png, gif).");
            }

            if (mensajes.Any())
                return false;

            // Llamada al método de datos
            bool resultado = objcd.CrearAcuerdo(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }

        public bool EditarAcuerdo(AcuerdoRebate obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (obj.IdAcuerdo <= 0)
                mensajes.Add("El acuerdo a editar no es válido.");

            if (string.IsNullOrWhiteSpace(obj.NombreAcuerdo))
                mensajes.Add("El nombre del acuerdo es obligatorio.");

            if (obj.ValorCriterio <= 0)
                mensajes.Add("El valor del criterio debe ser mayor a cero.");

            if (obj.Ganancia <= 0)
                mensajes.Add("La ganancia debe ser mayor a cero.");

            if (obj.FechaInicio == default)
                mensajes.Add("Debe seleccionar la Fecha Inicio.");

            if (obj.FechaFin == default)
                mensajes.Add("Debe seleccionar la Fecha Fin.");

            if (obj.FechaInicio > obj.FechaFin)
                mensajes.Add("La fecha de inicio no puede ser mayor que la fecha de fin.");

            if (obj.IdProveedor <= 0)
                mensajes.Add("Debe seleccionar un proveedor.");

            if (obj.IdModalidadOp <= 0)
                mensajes.Add("Debe seleccionar una modalidad de operación.");

            if (obj.IdTipoRebate <= 0)
                mensajes.Add("Debe seleccionar un tipo de rebate.");

            if (obj.IdCriterio <= 0)
                mensajes.Add("Debe seleccionar un criterio.");

            if (obj.IdPais <= 0)
                mensajes.Add("Debe seleccionar un país.");

            if (obj.IdMoneda <= 0)
                mensajes.Add("Debe seleccionar una moneda.");

            // Documento obligatorio
            if (string.IsNullOrWhiteSpace(obj.Documento))
                mensajes.Add("Debe adjuntar un documento para el acuerdo.");
            else
            {
                string ext = System.IO.Path.GetExtension(obj.Documento).ToLower();
                var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif" };
                if (!extensionesPermitidas.Contains(ext))
                    mensajes.Add("El documento debe ser PDF o imagen (jpg, png, gif).");
            }

            if (mensajes.Any())
                return false;

            bool resultado = objcd.EditarAcuerdo(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }
    }
}
