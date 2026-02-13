using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato
{
    public class CD_SubMenu
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<SubMenu> ObtenerSubMenusPorModulo()
        {
            List<SubMenu> lista = new List<SubMenu>();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IDSUBMENU, NOMBRESUBMENU FROM SUBMENU";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SubMenu
                    {
                        IdSubMenu = Convert.ToInt32(dr["IDSUBMENU"]),
                        NombreSubMenu = dr["NOMBRESUBMENU"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}
