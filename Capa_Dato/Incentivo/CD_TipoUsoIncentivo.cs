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
    public class CD_TipoUsoIncentivo
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<TipoUsoIncentivo> ObtenerTiposUsoIncentivo()
        {
            List<TipoUsoIncentivo> lista = new List<TipoUsoIncentivo>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_TIPO_USO_INCENTIVO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new TipoUsoIncentivo
                    {
                        IdTipoUsoIncentivo = Convert.ToInt32(dr["ID_TIPO_USO"]),
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

        public bool CrearUso(TipoUsoIncentivo obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new SqlConnection(cadenaConexion);
                SqlCommand cmd = new SqlCommand("SP_CREAR_TIPO_USO_INCENTIVO", conn);
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

        public bool EditarUso(TipoUsoIncentivo obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new SqlConnection(cadenaConexion);
                SqlCommand cmd = new SqlCommand("SP_EDITAR_TIPO_USO_INCENTIVO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_TIPO_USO", obj.IdTipoUsoIncentivo);
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
