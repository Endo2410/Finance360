using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CE_Incentivo
{
    public class DetalleIncentivoColaborador
    {
        public int Id { get; set; }

        public int IdMovimiento { get; set; }

        public int IdSucursal { get; set; }

        public int IdColaborador { get; set; }

        public string NombreColaborador { get; set; }

        public string Cargo { get; set; }

        public decimal MontoVendido { get; set; }

        public decimal PorcentajeComision { get; set; }

        public decimal MontoIncentivo { get; set; }
    }
}
