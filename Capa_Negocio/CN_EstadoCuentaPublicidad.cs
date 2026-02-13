using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_EstadoCuentaPublicidad
    {
        private readonly CD_EstadoCuentaPublicidad objcd = new();

        public List<EstadoCuentaPublicidad> ObtenerPendientes()
        {
            return objcd.ObtenerPendientes();
        }
    }
}
