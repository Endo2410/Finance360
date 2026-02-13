using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public bool Activo { get; set; }

        public Rol oRol { get; set; }
        public Modulo oModulo { get; set; }
        public SubMenu oSubMenu { get; set; }
        public Accion oAccion { get; set; }
        public bool Asignado { get; set; }
    }
}
