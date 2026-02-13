using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato
{
    public class CD_Modulo
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Modulo> ObtenerModulos()
        {
            List<Modulo> lista = new List<Modulo>();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IDMODULO, NOMBREMODULO FROM MODULO";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Modulo
                    {
                        IdModulo = Convert.ToInt32(dr["IDMODULO"]),
                        NombreModulo = dr["NOMBREMODULO"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}
