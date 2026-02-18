using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad.Contabilidad_Alejandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Contabilidad_Alejandra
{
    public class CN_CxP_Contabilidad_Alejandra
    {
        private readonly CD_CxP_Contabilidad_Alejandra objcd = new CD_CxP_Contabilidad_Alejandra();
        public List<E_CxP_Contabilidad_Alejandra> Listar()
        {
            return objcd.Listar();
        }

        public bool Guardar(E_CxP_Contabilidad_Alejandra obj, out string mensaje)
        {
          

            return objcd.Guardar(obj, out mensaje);
        }
    }
}
