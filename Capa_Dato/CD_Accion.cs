using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Entidad;

namespace Capa_Dato
{
    public class CD_Accion
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Accion> ObtenerAccionesPorSubMenu(int idSubMenu)
        {
            List<Accion> lista = new List<Accion>();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IDACCION, NOMBREACCION FROM ACCION";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Accion
                    {
                        IdAccion = Convert.ToInt32(dr["IDACCION"]),
                        NombreAccion = dr["NOMBREACCION"].ToString()
                    });
                }
            }
            return lista;
        }

    }
}
