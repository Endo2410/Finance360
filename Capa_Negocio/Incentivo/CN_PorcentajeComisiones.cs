using Capa_Dato.Incentivo;
using Capa_Entidad.CE_Incentivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Incentivo
{
    public class CN_PorcentajeComisiones
    {
        private readonly CD_PorcentajeComisiones cd = new();

        public List<PorcentajeComisiones> Obtener() => cd.Obtener();

        public bool Crear(PorcentajeComisiones obj, out List<string> mensajes)
        {
            mensajes = new();
            if (string.IsNullOrWhiteSpace(obj.Cargo))
                mensajes.Add("El cargo es obligatorio.");
            if (obj.MontoMin < 0 || obj.MontoMax <= 0 || obj.MontoMax < obj.MontoMin)
                mensajes.Add("Rango de monto inválido.");
            if (obj.Porcentaje <= 0)
                mensajes.Add("El porcentaje debe ser mayor a 0.");

            if (mensajes.Any()) return false;

            bool ok = cd.Crear(obj, out string msg);
            mensajes.Add(ok ? "Registro creado correctamente." : msg);
            return ok;
        }

        public bool Editar(PorcentajeComisiones obj, out List<string> mensajes)
        {
            mensajes = new();
            if (string.IsNullOrWhiteSpace(obj.Cargo))
                mensajes.Add("El cargo es obligatorio.");
            if (obj.MontoMin < 0 || obj.MontoMax <= 0 || obj.MontoMax < obj.MontoMin)
                mensajes.Add("Rango de monto inválido.");
            if (obj.Porcentaje <= 0)
                mensajes.Add("El porcentaje debe ser mayor a 0.");

            if (mensajes.Any()) return false;

            bool ok = cd.Editar(obj, out string msg);
            mensajes.Add(ok ? "Registro actualizado correctamente." : msg);
            return ok;
        }
    }
}
