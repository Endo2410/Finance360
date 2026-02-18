using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad.Contabilidad_Alejandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.Contabilidad_Alejandra
{
    public class CN_TipoServicio
    {
        private readonly CD_TipoServicio objcd = new CD_TipoServicio();
        public List<E_TipoServicio> ObtenerTipoServicio() => objcd.ObtenerTipoServicio();

        public bool Guardar(E_TipoServicio obj, out string mensaje)
        {
            return objcd.Guardar(obj, out mensaje);
        }

    }
}
