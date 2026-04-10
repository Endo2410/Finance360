using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CE_Rebate
{
    public class CriterioRebate
    {
        public int IdCriterio { get; set; }
        public string Nombre { get; set; }
        public string Operador { get; set; }
        public string Descripcion { get; set; }
        public int IdEstado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Estado oEstado { get; set; }
    }
}
