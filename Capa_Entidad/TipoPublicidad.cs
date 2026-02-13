using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class TipoPublicidad
    {
        public int IdTipoPublicidad { get; set; }
        public string Nombre { get; set; }
        public int IdEstado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Estado oEstado { get; set; }
    }
}
