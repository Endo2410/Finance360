using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Moneda
    {
        public int IdMoneda { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public int IdEstado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Estado oEstado { get; set; }
    }
}
