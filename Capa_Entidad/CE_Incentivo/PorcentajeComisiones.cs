using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CE_Incentivo
{
    public class PorcentajeComisiones
    {
        public int Id { get; set; }
        public string Cargo { get; set; }
        public decimal MontoMin { get; set; }
        public decimal MontoMax { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
