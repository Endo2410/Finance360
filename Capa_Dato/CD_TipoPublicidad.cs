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
    public class CD_TipoPublicidad
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<TipoPublicidad> ObtenerTiposPublicidad()
        {
            List<TipoPublicidad> lista = new List<TipoPublicidad>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_TIPO_PUBLICIDAD", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new TipoPublicidad
                    {
                        IdTipoPublicidad = Convert.ToInt32(dr["ID_TIPO_PUBLICIDAD"]),
                        Nombre = dr["NOMBRE"].ToString(),
                        IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["ESTADO"].ToString()
                        }
                    });
                }
            }
            return lista;
        }

        public bool Crear(TipoPublicidad obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new SqlConnection(cadenaConexion);
                SqlCommand cmd = new SqlCommand("SP_CREAR_TIPO_PUBLICIDAD", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@IDESTADO", obj.oEstado.IdEstado);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool Editar(TipoPublicidad obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new SqlConnection(cadenaConexion);
                SqlCommand cmd = new SqlCommand("SP_EDITAR_TIPO_PUBLICIDAD", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_TIPO_PUBLICIDAD", obj.IdTipoPublicidad);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@IDESTADO", obj.oEstado.IdEstado);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }
    }

}
