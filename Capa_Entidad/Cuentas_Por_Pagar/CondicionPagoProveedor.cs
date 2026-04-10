using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Cuentas_Por_Pagar
{
    public class CondicionPagoProveedor
    {
        public int IdCondicion { get; set; }

        public int IdProveedor { get; set; }
        public int DiasCredito { get; set; }

        public int? IdTipoRetencion { get; set; }
        public int? IdTipoDescuento { get; set; }

        public Proveedor oProveedor { get; set; }
        public TipoRetencion oTipoRetencion { get; set; }
        public TipoDescuentoPP oTipoDescuento { get; set; }
    }
}
