using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_Modulo
    {
        private readonly CD_Modulo objCD = new CD_Modulo();
        public List<Modulo> ObtenerModulos() => objCD.ObtenerModulos();
    }
}
