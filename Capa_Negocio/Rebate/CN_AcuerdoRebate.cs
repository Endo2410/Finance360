using Capa_Dato.Rebate;
using Capa_Entidad.CE_Rebate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Rebate
{
    public class CN_AcuerdoRebate
    {
        private readonly CD_AcuerdoRebate objcd = new CD_AcuerdoRebate();

        public List<AcuerdoRebate> ObtenerAcuerdos()
        {
            return objcd.ObtenerAcuerdos();
        }

        public List<DetalleAcuerdo> ObtenerDetalles(int idAcuerdo)
        {
            if (idAcuerdo <= 0)   return new List<DetalleAcuerdo>(); 

            return objcd.ObtenerDetalles(idAcuerdo);
        }


        public bool CrearAcuerdo(AcuerdoRebate obj, List<DetalleAcuerdo> detalles, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // =========================
            // VALIDACIONES CABECERA
            // =========================

            if (string.IsNullOrWhiteSpace(obj.NombreAcuerdo))
                mensajes.Add("El nombre del acuerdo es obligatorio.");

            // =========================
            // VALIDACIÓN VALOR CRITERIO
            // =========================

            if (obj.IdTipoRebate == 4)
            {
                if (detalles == null || !detalles.Any())
                    mensajes.Add("Debe seleccionar al menos un item.");

                if (detalles.Any(d => d.Cantidad == null || d.Cantidad <= 0))
                    mensajes.Add("Todos los items deben tener cantidad válida.");
            }
            else if (obj.IdTipoRebate == 3)
            {
                if (detalles == null || !detalles.Any())
                    mensajes.Add("Debe seleccionar al menos un item.");

                if (detalles.Any(d => d.Porcentaje == null || d.Porcentaje <= 0))
                    mensajes.Add("Todos los items deben tener porcentaje válido.");

                if (detalles.Any(d => d.PrecioBase == null || d.PrecioBase <= 0))
                    mensajes.Add("Todos los items deben tener precio base válido.");
            }
            else
            {
                if (obj.ValorCriterio <= 0)
                    mensajes.Add("El valor del criterio debe ser mayor a cero.");
            }

            if (obj.IdTipoRebate != 3 && obj.Ganancia <= 0)
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

            // =========================
            // VALIDACIÓN DOCUMENTO
            // =========================

            if (string.IsNullOrWhiteSpace(obj.Documento))
                mensajes.Add("Debe adjuntar un documento para el acuerdo.");
            else
            {
                string ext = Path.GetExtension(obj.Documento).ToLower();
                var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif" };

                if (!extensionesPermitidas.Contains(ext))
                    mensajes.Add("El documento debe ser PDF o imagen (jpg, png, gif).");
            }

        
            // =========================
            // SI HAY ERRORES → STOP
            // =========================
            if (mensajes.Any())
                return false;

            // =========================
            // LLAMAR CAPA DATOS
            // =========================
            bool resultado = objcd.CrearAcuerdo(obj, detalles, out string msg);

            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }

        public bool EditarAcuerdo(AcuerdoRebate obj, List<DetalleAcuerdo> detalles, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (obj.IdAcuerdo <= 0)
                mensajes.Add("El acuerdo a editar no es válido.");

            if (string.IsNullOrWhiteSpace(obj.NombreAcuerdo))
                mensajes.Add("El nombre del acuerdo es obligatorio.");
            if (obj.IdTipoRebate == 4)
            {
                if (detalles == null || !detalles.Any())
                    mensajes.Add("Debe seleccionar al menos un item.");

                if (detalles.Any(d => d.Cantidad == null || d.Cantidad <= 0))
                    mensajes.Add("Todos los items deben tener cantidad válida.");
            }
            else if (obj.IdTipoRebate == 3)
            {
                if (detalles == null || !detalles.Any())
                    mensajes.Add("Debe seleccionar al menos un item.");

                if (detalles.Any(d => d.Porcentaje == null || d.Porcentaje <= 0))
                    mensajes.Add("Todos los items deben tener porcentaje válido.");

                if (detalles.Any(d => d.PrecioBase == null || d.PrecioBase <= 0))
                    mensajes.Add("Todos los items deben tener precio base válido.");
            }
            else
            {
                if (obj.ValorCriterio <= 0)
                    mensajes.Add("El valor del criterio debe ser mayor a cero.");
            }

            if (obj.IdTipoRebate != 3 && obj.Ganancia <= 0)
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

            if (string.IsNullOrWhiteSpace(obj.Documento))
                mensajes.Add("Debe adjuntar un documento para el acuerdo.");
            else
            {
                string ext = Path.GetExtension(obj.Documento).ToLower();
                var extensionesPermitidas = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif" };
                if (!extensionesPermitidas.Contains(ext))
                    mensajes.Add("El documento debe ser PDF o imagen (jpg, png, gif).");
            }

            if (mensajes.Any())
                return false;

            // Llamada a capa de datos con detalles
            bool resultado = objcd.EditarAcuerdo(obj, detalles, out string msg);
            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }
    }
}
