using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Cuentas_Por_Pagar
{
    public class TipoDescuentoPP
    {
        public int IdTipoDescuento { get; set; }

        public string Nombre { get; set; }

        public decimal Porcentaje { get; set; }

        public int IdEstado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public Estado oEstado { get; set; }
    }
}
