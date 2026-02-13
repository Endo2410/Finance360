using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class TipoRebate
    {
        public int IdTipoRebate { get; set; }
        public string Nombre { get; set; }
        public int IdEstado { get; set; }
        public Estado oEstado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
