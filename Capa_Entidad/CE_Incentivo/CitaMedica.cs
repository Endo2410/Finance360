using Capa_Entidad.Contabilidad_Alejandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CE_Incentivo
{
    public class CitaMedica
    {
        public int IdCita { get; set; }

        public string NombreCita { get; set; }

        public string DocumentoAdjunto { get; set; }

        public int IdEstado { get; set; }

        public int IdDepartamento { get; set; }

        public int IdSucursal { get; set; }

        public string UsuarioRegistro { get; set; }

        public DateTime FechaRegistro { get; set; }

        public List<DateTime> Fechas { get; set; } = new List<DateTime>();

        public Departamento oDepartamento { get; set; }

        public E_Sucursales oSucursal { get; set; }
        public Estado oEstado { get; set; }
    }
}
