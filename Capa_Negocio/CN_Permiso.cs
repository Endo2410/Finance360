using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio
{
    public class CN_Permiso
    {
        private readonly CD_Permiso objcd = new CD_Permiso();

        public List<Permiso> ObtenerEstructuraCompleta()
        {
            return objcd.ObtenerEstructuraCompleta();
        }

        public PermisoRolDto ObtenerPermisosPorRol(int idRol)
        {
            return objcd.ObtenerPermisosPorRol(idRol);
        }

        public bool GuardarPermisos(int idRol, List<int> acciones, List<int> subMenus, List<int> modulos)
        {
            return objcd.GuardarPermisos(idRol, acciones, subMenus, modulos);
        }

        public bool GuardarPermisosUsuario(int idUsuario, List<int> acciones, List<int> subMenus, List<int> modulos)
        {
            return objcd.GuardarPermisosUsuario(idUsuario, acciones, subMenus, modulos);
        }



        public List<Permiso> ObtenerEstructuraPorUsuario(int idUsuario)
        {
            // Primero obtienes el rol del usuario
            var cn_usuario = new CN_Usuario();
            var usuario = cn_usuario.ObtenerUsuarios().FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null) return new List<Permiso>();

            // Luego obtienes los permisos por el rol
            var cd_permiso = new CD_Permiso();
            return cd_permiso.ObtenerModulosYSubMenusPorRol(usuario.IdRol);
        }

        public List<string> ObtenerAccionesPorUsuario(int idUsuario)
        {
            return objcd.ObtenerAccionesPorUsuario(idUsuario);
        }

        public PermisoRolDto ObtenerPermisosPorUsuario(int idUsuario)
        {
            return objcd.ObtenerPermisosPorUsuario(idUsuario);
        }
    }
}
