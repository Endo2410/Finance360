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
    public class CD_TipoServicio
    {
        private readonly string cn = Conexion.cn;
        public List<E_TipoServicio> ObtenerTipoServicio()
        {
            List<E_TipoServicio> lista = new List<E_TipoServicio>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_TIPO_SERVICIOS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new E_TipoServicio
                        {
                            IdTipoServicio = Convert.ToInt32(dr["ID_TIPO_SERVICIO"]),
                            DescripcionServicio = dr["DESCRIPCION_SERVICIO"].ToString(),
                            UsuarioCreador = dr["USUARIO_CREADOR"].ToString(),
                            UsuarioModificador = dr["USUARIO_MODIFICADOR"].ToString(),
                            FechaCreacion =Convert.ToDateTime (dr["FECHA_CREACION"]),
                            FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(dr["FECHA_MODIFICACION"]),



                        });
                    }
                }
            }

            return lista;
        }


        public bool Guardar(E_TipoServicio obj, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_GUARDAR_TIPO_SERVICIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_TIPO_SERVICIO", obj.IdTipoServicio);
                cmd.Parameters.AddWithValue("@DESCRIPCION_SERVICIO", obj.DescripcionServicio);
                cmd.Parameters.AddWithValue("@ID_USUARIO", obj.IdUsuario);

                cmd.Parameters.Add("@RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                respuesta = Convert.ToBoolean(cmd.Parameters["@RESULTADO"].Value);
                mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
            }

            return respuesta;
        }

    }
}
