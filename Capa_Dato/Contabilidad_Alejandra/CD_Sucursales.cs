using Capa_Entidad;
using Capa_Entidad.Contabilidad_Alejandra;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.Contabilidad_Alejandra
{
    public class CD_Sucursales
    {
        private readonly string cn = Conexion.cn;
        public List<E_Sucursales> ObtenerSucursales()
        {
            List<E_Sucursales> lista = new List<E_Sucursales>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_SUCURSALES", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new E_Sucursales
                        {
                            IdSucursal = Convert.ToInt32(dr["ID_SUCURSAL"]),
                            NombreSucursal = dr["NOMBRE_SUCURSAL"].ToString(),
                            Codigo = dr["CODIGO"].ToString(),
                         
                        });
                    }
                }
            }

            return lista;
        }


        public (int insertados, int actualizados) SincronizarSucursales()
        {
            int insertados = 0;
            int actualizados = 0;

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_SINCRONIZAR_SUCURSALES", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        insertados = Convert.ToInt32(dr["Insertados"]);
                        actualizados = Convert.ToInt32(dr["Actualizados"]);
                    }
                }
            }

            return (insertados, actualizados);
        }



    }
}
