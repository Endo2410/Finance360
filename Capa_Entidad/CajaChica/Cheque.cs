using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CajaChica
{
    public class Cheque
    {
        public int IdCheque { get; set; }
        public int NumeroCheque { get; set; }
        public string Concepto { get; set; }
        public decimal Entrada { get; set; }
        public int IdUsuario { get; set; } // FK a la tabla Usuario
        public DateTime FechaRegistro { get; set; }
        public byte[] Foto { get; set; }
    }
}
