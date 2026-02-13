using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato
{
    public class CD_Pais
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Pais> ObtenerPaises()
        {
            List<Pais> lista = new List<Pais>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_PAIS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Pais p = new Pais
                    {
                        IdPais = Convert.ToInt32(dr["ID_PAIS"]),
                        Nombre = dr["NOMBRE"].ToString(),
                        IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["ESTADO"].ToString()
                        }
                    };
                    lista.Add(p);
                }
            }

            return lista;
        }

        public bool CrearPais(Pais obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_PAIS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool EditarPais(Pais obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_PAIS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
