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
    public class CN_CampaniaPublicitaria
    {
        private readonly CD_CampaniaPublicitaria objcd = new CD_CampaniaPublicitaria();

        public List<CampaniaPublicitaria> ObtenerCampanias()
        {
            return objcd.ObtenerCampanias();
        }

        public bool CrearCampania(CampaniaPublicitaria obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (string.IsNullOrWhiteSpace(obj.NombreCampania))
                mensajes.Add("El nombre de la campaña es obligatorio.");
            if (obj.MontoInversion <= 0)
                mensajes.Add("El monto de inversión debe ser mayor a cero.");
            if (obj.FechaInicio > obj.FechaFin)
                mensajes.Add("La fecha de inicio no puede ser mayor a la fecha de fin.");

            if (mensajes.Any())
                return false;

            bool resultado = objcd.CrearCampania(obj, out string msg);

            if (!resultado) mensajes.Add(msg);
            return resultado;
        }

        public bool EditarCampania(CampaniaPublicitaria obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (string.IsNullOrWhiteSpace(obj.NombreCampania))
                mensajes.Add("El nombre de la campaña es obligatorio.");
            if (obj.MontoInversion <= 0)
                mensajes.Add("El monto de inversión debe ser mayor a cero.");
            if (obj.FechaInicio > obj.FechaFin)
                mensajes.Add("La fecha de inicio no puede ser mayor a la fecha de fin.");

            if (mensajes.Any())
                return false;

            bool resultado = objcd.EditarCampania(obj, out string msg);

            if (!resultado) mensajes.Add(msg);
            return resultado;
        }

        public List<CampaniaPublicitaria> ObtenerCampaniasResumen()
        {
            return objcd.ObtenerCampaniasResumen();
        }

        public List<DetallePagoCampania> ObtenerDetallePagoCampania(int idCampania)
        {
            return objcd.ObtenerDetallePagoCampania(idCampania);
        }

        public bool AnularPagoPublicidad(int idDetallePago, string usuario)
        {
            if (idDetallePago <= 0)
                return false;

            return objcd.AnularPagoPublicidad(idDetallePago, usuario);
        }

    }
}
