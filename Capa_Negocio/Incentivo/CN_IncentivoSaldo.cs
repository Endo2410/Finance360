using Capa_Dato.Incentivo;
using Capa_Entidad.CE_Incentivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

        public bool RegistrarUsoIncentivo(IncentivoMovimiento obj)
        {
            return cd.RegistrarUsoIncentivo(obj);
        }

        public List<DetalleIncentivoColaborador> ObtenerDetalleColaboradores(int idMovimiento)
        {
            return cd.ObtenerDetalleColaboradores(idMovimiento);
        }

      
        // Obtener usos del incentivo por sucursal
        public List<IncentivoMovimiento> ObtenerUsos(int idSucursal)
        {
            return cd.ObtenerUsos(idSucursal);
        }

        public List<ColaboradorDTO> ObtenerColaboradores(int idSucursal)
        {
            return cd.ObtenerColaboradores(idSucursal);
        }

    }
}
