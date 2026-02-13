using Capa_Dato;
using Capa_Entidad;
using System.Collections.Generic;

namespace Capa_Negocio
{
    public class CN_Proveedor
    {
        private readonly CD_Proveedor objcd = new CD_Proveedor();

        public List<Proveedor> ObtenerProveedores()
        {
            return objcd.ObtenerProveedores();
        }

        public object SincronizarProveedores()
        {
            return objcd.SincronizarProveedores();
        }

    }
}
