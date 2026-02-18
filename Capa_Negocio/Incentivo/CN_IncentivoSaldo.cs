using Capa_Dato.Incentivo;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capa_Negocio.Incentivo
{
    public class CN_IncentivoSaldo
    {
        private readonly CD_IncentivoSaldo cd =
            new CD_IncentivoSaldo();

        public IncentivoSaldo ObtenerSaldo(int idSucursal)
        {
            return cd.ObtenerSaldo(idSucursal);
        }

        public List<IncentivoRecibido> ObtenerIncentivosRecibidos(int idSucursal)
        {
            return cd.ObtenerIncentivosRecibidos(idSucursal);
        }

        public bool Registrar(IncentivoMovimiento obj)
        {
            return cd.Registrar(obj);
        }

        // Obtener usos del incentivo por sucursal
        public List<IncentivoMovimiento> ObtenerUsos(int idSucursal)
        {
            return cd.ObtenerUsos(idSucursal);
        }
    }
}
