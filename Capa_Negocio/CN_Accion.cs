using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Dato;
using Capa_Entidad;

namespace Capa_Negocio
{
    public class CN_Accion
    {
        private readonly CD_Accion objCD = new CD_Accion();
        public List<Accion> ObtenerAccionesPorSubMenu(int idSubMenu) => objCD.ObtenerAccionesPorSubMenu(idSubMenu);
    }
}
