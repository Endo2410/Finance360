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
    public class CD_Estado
    {
        private readonly string cn = Conexion.cn;
        public List<Estado> ObtenerEstado()
        {
            List<Estado> lista = new List<Estado>();
            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SELECT IdEstado, Nombre FROM Estado", conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Estado
                    {
                        IdEstado = Convert.ToInt32(dr["IdEstado"]),
                        Nombre = dr["Nombre"].ToString()
                    });
                }
            }
            return lista;
        }

        public List<Estado> ObtenerEstados()
        {
            List<Estado> lista = new List<Estado>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ESTADO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["NOMBRE"].ToString(),
                            Descripcion = dr["DESCRIPCION"].ToString(),
                            Modulo = dr["MODULO"]?.ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHAREGISTRO"])
                        });
                    }
                }
            }

            return lista;
        }

        public bool CrearEstado(Estado obj, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_ESTADO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@DESCRIPCION", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MODULO", obj.Modulo ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool EditarEstado(Estado obj, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_ESTADO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@DESCRIPCION", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MODULO", obj.Modulo ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }
    }
}
