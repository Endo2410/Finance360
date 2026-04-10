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

        //DEPARTAMENTO
        public List<Departamento> ObtenerDepartamentos()
        {
            return objcd.ObtenerDepartamentos();
        }

        public object SincronizarDepartamentos()
        {
            return objcd.SincronizarDepartamentos();
        }

        public List<ItemDepartamento> LISTAR_ITEM(int? ID_DEPARTAMENTO)
        {
            return objcd.LISTAR_ITEM(ID_DEPARTAMENTO);
        }



    }
}
