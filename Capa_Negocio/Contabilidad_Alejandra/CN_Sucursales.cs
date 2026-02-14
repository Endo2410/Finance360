using Capa_Dato;
using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad;
using Capa_Entidad.Contabilidad_Alejandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio.Contabilidad_Alejandra
{
    public class CN_Sucursales
    {
        private readonly CD_Sucursales objcd = new CD_Sucursales();
        public List<E_Sucursales> ObtenerSucursales() => objcd.ObtenerSucursales();

        public (int insertados, int actualizados) SincronizarSucursales()
        {
            return objcd.SincronizarSucursales();
        }
    }
}
