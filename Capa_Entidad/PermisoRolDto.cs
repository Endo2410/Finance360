using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class PermisoRolDto
    {
        public int IdRol { get; set; }
        public List<int> IdsPermisos { get; set; }


        public List<int> Acciones { get; set; } = new();
        public List<int> SubMenus { get; set; } = new();
        public List<int> Modulos { get; set; } = new();
    }
}
