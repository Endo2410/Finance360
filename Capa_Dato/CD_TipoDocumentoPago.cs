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
    public class CD_TipoDocumentoPago
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<TipoDocumentoPago> ObtenerTipos()
        {
            List<TipoDocumentoPago> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_TIPO_DOC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoDocumentoPago
                    {
                        IdTipoDoc = Convert.ToInt32(dr["ID_TIPO_DOC"]),
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

        public bool CrearTipo(TipoDocumentoPago obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_TIPO_DOC", conn);
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

        public bool EditarTipo(TipoDocumentoPago obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_TIPO_DOC", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_TIPO_DOC", obj.IdTipoDoc);
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
