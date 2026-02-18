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
    public class CD_CxP_Contabilidad_Alejandra
    {
        private readonly string cn = Conexion.cn;
        public List<E_CxP_Contabilidad_Alejandra> Listar()
        {
            List<E_CxP_Contabilidad_Alejandra> lista = new();

            using (SqlConnection con = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_CXP_CONTABILIDAD_ALEJANDRA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new E_CxP_Contabilidad_Alejandra()
                        {
                            IdCxP = Convert.ToInt32(dr["ID_CxP"]),
                            IdCliente = Convert.ToInt32(dr["ID_CLIENTE"]),
                            IdMesxPagar = Convert.ToInt32(dr["ID_TIPO_CANJE"]),
                            FechaVencimiento = Convert.ToDateTime(dr["FECHA_VENCIMIENTO"]),
                            MontoxPagar = Convert.ToDecimal(dr["MONTOxPAGAR"]),
                            Observaciones = dr["OBSERVACIONES"]?.ToString(),
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),

                            NumeroCliente = dr["NUMERO_CLIENTE"]?.ToString(),
                            NombreCliente = dr["NOMBRE_CLIENTE"]?.ToString(),
                            TipoServicio = dr["DESCRIPCION_SERVICIO"]?.ToString(),
                            Sucursal = dr["NOMBRE_SUCURSAL"]?.ToString(),
                            MesDescripcion = dr["MES"]?.ToString(),
                            Estado = dr["ESTADO"]?.ToString(),
                            UsuarioCreador = dr["USUARIO_CREADOR"]?.ToString(),
                            UsuarioModificador = dr["USUARIO_MODIFICADOR"]?.ToString(),
                            FechaCreacion = dr["FECHA_CREACION"] as DateTime?,
                            FechaModificacion = dr["FECHA_MODIFICACION"] as DateTime?,

                            // =============================
                            // NUEVOS CAMPOS DE ARCHIVOS
                            // =============================

                            TieneReciboPendiente = Convert.ToBoolean(dr["TieneReciboPendiente"]),
                            TieneReciboPagado = Convert.ToBoolean(dr["TieneReciboPagado"]),

                            RutaReciboPendiente = dr["RutaReciboPendiente"] == DBNull.Value
                                                    ? null
                                                    : dr["RutaReciboPendiente"].ToString(),

                            RutaReciboPagado = dr["RutaReciboPagado"] == DBNull.Value
                                                    ? null
                                                    : dr["RutaReciboPagado"].ToString()
                        });
                    }

                }
            }

            return lista;
        }

        public bool Guardar(E_CxP_Contabilidad_Alejandra obj, out string mensaje)
        {
            bool respuesta = false;
            mensaje = "";

            using (SqlConnection con = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_GUARDAR_CXP_CONTABILIDAD_ALEJANDRA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //  ESTE ES EL CAMBIO IMPORTANTE
                SqlParameter paramId = new SqlParameter("@ID_CxP", SqlDbType.Int);
                paramId.Value = obj.IdCxP;
                paramId.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(paramId);

                cmd.Parameters.AddWithValue("@ID_CLIENTE", obj.IdCliente);
                cmd.Parameters.AddWithValue("@ID_MESxPAGAR", obj.IdMesxPagar);
                cmd.Parameters.AddWithValue("@FECHA_VENCIMIENTO", obj.FechaVencimiento);
                cmd.Parameters.AddWithValue("@MONTOxPAGAR", obj.MontoxPagar);
                cmd.Parameters.AddWithValue("@OBSERVACIONES", obj.Observaciones ?? "");
                cmd.Parameters.AddWithValue("@ID_ESTADO", obj.IdEstado);
                cmd.Parameters.AddWithValue("@ID_USUARIO", obj.IdUsuario);

                cmd.Parameters.Add("@RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                //  AQUÍ ACTUALIZAMOS EL ID GENERADO
                obj.IdCxP = Convert.ToInt32(cmd.Parameters["@ID_CxP"].Value);

                respuesta = Convert.ToBoolean(cmd.Parameters["@RESULTADO"].Value);
                mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
            }

            return respuesta;
        }

    }
}
