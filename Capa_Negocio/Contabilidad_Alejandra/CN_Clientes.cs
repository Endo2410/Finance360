using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad.Contabilidad_Alejandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Contabilidad_Alejandra
{
    public class CN_Clientes
    {
        private readonly CD_Clientes objcd = new CD_Clientes();
        public List<E_Clientes> ObtenerClientes() => objcd.ObtenerClientes();

        public bool Guardar(E_Clientes obj, out string mensaje)
        {
            return objcd.Guardar(obj, out mensaje);
        }
    }
}
