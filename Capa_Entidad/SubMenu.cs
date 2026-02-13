using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class SubMenu
    {
        public int IdSubMenu { get; set; }
        public int IdModulo { get; set; }
        public string NombreSubMenu { get; set; }

        public string Controlador { get; set; } // Ej: "Solicitud"
        public string Accion { get; set; }       // Ej: "Index"

    }
}
