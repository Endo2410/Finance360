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
    public class CD_Clientes
    {
        private readonly string cn = Conexion.cn;
        public List<E_Clientes> ObtenerClientes()
        {

            try
            {
                List<E_Clientes> lista = new List<E_Clientes>();

                using (SqlConnection conn = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_OBTENER_CLIENTES_SERVICIOS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new E_Clientes
                            {
                                IdCliente = Convert.ToInt32(dr["ID_CLIENTE"]),
                                NumeroCliente = Convert.ToInt32(dr["NUMERO_CLIENTE"]),
                                NombreCliente = dr["NOMBRE_CLIENTE"].ToString(),
                                TipoServicio = dr["DESCRIPCION_SERVICIO"].ToString(),
                                Sucursal = dr["NOMBRE_SUCURSAL"] == DBNull.Value
                                ? null
                                : dr["NOMBRE_SUCURSAL"].ToString(),
                                Estado = dr["ESTADO"].ToString(),
                                UsuarioCreador = dr["USUARIO_CREADOR"].ToString(),
                                UsuarioModificador = dr["USUARIO_MODIFICADOR"].ToString(),
                                FechaCreacion = Convert.ToDateTime(dr["FECHA_CREACION"]),
                                FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(dr["FECHA_MODIFICACION"]),
                                IdTipoServicio = Convert.ToInt32(dr["ID_TIPO_SERVICIO"]),
                                IdSucursal = dr["ID_SUCURSAL"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(dr["ID_SUCURSAL"]),

                                IdEstado = Convert.ToInt32(dr["ID_ESTADO"]),




                            });
                        }
                    }
                }

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
          
        }


        public bool Guardar(E_Clientes obj, out string mensaje)
        {
            bool respuesta = false;
            mensaje = string.Empty;

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_GUARDAR_CLIENTES_SERVICIOS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_CLIENTE", obj.IdCliente);
                cmd.Parameters.AddWithValue("@NUMERO_CLIENTE", obj.NumeroCliente);
                cmd.Parameters.AddWithValue("@NOMBRE_CLIENTE", obj.NombreCliente);
                cmd.Parameters.AddWithValue("@ID_TIPO_SERVICIO", obj.IdTipoServicio);
                cmd.Parameters.AddWithValue("@ID_SUCURSAL",
                obj.IdSucursal == 0 ? (object)DBNull.Value : obj.IdSucursal);

                cmd.Parameters.AddWithValue("@ID_ESTADO", obj.IdEstado);
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
