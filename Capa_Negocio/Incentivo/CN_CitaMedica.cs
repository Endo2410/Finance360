using Capa_Dato.Incentivo;
using Capa_Entidad.CE_Incentivo;

namespace Capa_Negocio.Incentivo
{
    public class CN_CitaMedica
    {
        private readonly CD_CitaMedica cd = new CD_CitaMedica();

        public List<CitaMedica> Obtener()
        {
            return cd.Obtener();
        }

        public bool Crear(CitaMedica obj, out List<string> mensajes)
        {
            mensajes = new();

            if (string.IsNullOrEmpty(obj.NombreCita))
                mensajes.Add("Debe ingresar el nombre de la cita.");

            if (obj.IdDepartamento <= 0)
                mensajes.Add("Debe seleccionar un departamento.");

            if (obj.IdSucursal <= 0)
                mensajes.Add("Debe seleccionar una sucursal.");

            if (obj.Fechas == null || obj.Fechas.Count == 0)
                mensajes.Add("Debe agregar al menos una fecha.");

            if (mensajes.Any())
                return false;

            bool ok = cd.Crear(obj, out string msg);

            if (!ok)
                mensajes.Add(msg);

            return ok;
        }

        public bool Editar(CitaMedica obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdCita <= 0)
                mensajes.Add("Cita inválida.");

            if (mensajes.Any())
                return false;

            bool ok = cd.Editar(obj, out string msg);

            if (!ok)
                mensajes.Add(msg);

            return ok;
        }

        public CitaMedica ObtenerCitaPorId(int id)
        {
            return cd.ObtenerCitaPorId(id);
        }

        public List<DateTime> ObtenerFechas(int idCita)
        {
            return cd.ObtenerFechas(idCita);
        }
    }
}
