using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Modalidad
    {
        public int IdModalidad { get; set; }
        public string Nombre { get; set; }

        public string TipoIntervalo { get; set; }   // DIA, MES, ANIO
        public int ValorIntervalo { get; set; }     // 1, 15, 3, etc.

        public int IdEstado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Estado oEstado { get; set; }
    }
}
