using Capa_Entidad.CajaChica;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.CajaChica
{
    public class CD_CajaChica
    {
        public List<Movimiento> Listar()
        {
            List<Movimiento> lista = new List<Movimiento>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    // Consulta directa o puedes crear un SP_LISTAR_MOVIMIENTOS
                    string query = "SELECT M.ID_MOVIMIENTO,M.NUM_VALE, M.NOMBRES_Y_APELLIDOS, M.CONCEPTO, M.ENTRADAS, M.SALIDAS, M.RETORNO_DINERO, M.MOTIVO, M.SALDO_ANTERIOR, M.SALDO_ACTUAL, M.IDUSUARIO,U.NOMBRES +' '+U.APELLIDOS AS USUARIO_CREADOR, M.FECHA_CREACION,M.FECHA_MODIFICACION,US.NOMBRES+' '+US.APELLIDOS AS USUARIO_AUTORIZADOR,M.ES_ANULADO, M.IDUSUARIO_AUTORIZADOR,M.MOTIVO_ANULADO,M.IDUSUARIO_ANULADOR,M.IDUSUARIO,UA.NOMBRES +' '+UA.APELLIDOS AS USUARIO_ANULADOR  FROM MOVIMIENTOS M INNER JOIN USUARIO U ON M.IDUSUARIO=U.IDUSUARIO LEFT JOIN USUARIO US ON M.IDUSUARIO_AUTORIZADOR=US.IDUSUARIO LEFT JOIN USUARIO UA ON M.IDUSUARIO_ANULADOR=UA.IDUSUARIO ORDER BY ID_MOVIMIENTO DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Movimiento()
                            {
                                IdMovimiento = dr["ID_MOVIMIENTO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ID_MOVIMIENTO"]),

                                NumVale = dr["NUM_VALE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NUM_VALE"]),

                                NombresApellidos = dr["NOMBRES_Y_APELLIDOS"] == DBNull.Value ? "" : dr["NOMBRES_Y_APELLIDOS"].ToString(),

                                Concepto = dr["CONCEPTO"] == DBNull.Value ? "" : dr["CONCEPTO"].ToString(),

                                // Los decimales suelen dar error si vienen NULL
                                Entradas = dr["ENTRADAS"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["ENTRADAS"]),
                                Salidas = dr["SALIDAS"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["SALIDAS"]),
                                RetornoDinero = dr["RETORNO_DINERO"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["RETORNO_DINERO"]),

                                Motivo = dr["MOTIVO"] == DBNull.Value ? "" : dr["MOTIVO"].ToString(),

                                SaldoAnterior = dr["SALDO_ANTERIOR"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["SALDO_ANTERIOR"]),
                                SaldoActual = dr["SALDO_ACTUAL"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["SALDO_ACTUAL"]),

                                IdUsuario = dr["IDUSUARIO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IDUSUARIO"]),

                                // Fechas: Importante usar DateTime? (nullable) en tu clase Entidad para evitar el año 0001
                                FechaCreacion = dr["FECHA_CREACION"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["FECHA_CREACION"]),

                                EsAnulado = dr["ES_ANULADO"] != DBNull.Value && Convert.ToBoolean(dr["ES_ANULADO"]),

                                UsuarioCreador = dr["USUARIO_CREADOR"] == DBNull.Value ? "Sistema" : dr["USUARIO_CREADOR"].ToString(),

                                IdUsuarioAutorizador = dr["IDUSUARIO_AUTORIZADOR"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IDUSUARIO_AUTORIZADOR"]),

                                UsuarioAutorizador = dr["USUARIO_AUTORIZADOR"] == DBNull.Value ? "" : dr["USUARIO_AUTORIZADOR"].ToString(),

                                // Si tu propiedad FechaModificacion es DateTime? (con el signo de pregunta)
                                FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FECHA_MODIFICACION"]),

                                MotivoAnulado = dr["MOTIVO_ANULADO"] == DBNull.Value ? "" : dr["MOTIVO_ANULADO"].ToString(),
                                IdUsuarioAnulador = dr["IDUSUARIO_ANULADOR"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IDUSUARIO_ANULADOR"]),
                                UsuarioAnulador = dr["USUARIO_ANULADOR"] == DBNull.Value ? "" : dr["USUARIO_ANULADOR"].ToString(),
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Movimiento>();
            }

            return lista;
        }
        public int RegistrarMovimiento(Movimiento obj, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_GuardarMovimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("NombresApellidos", obj.NombresApellidos);
                    cmd.Parameters.AddWithValue("Concepto", obj.Concepto);
                    cmd.Parameters.AddWithValue("Entradas", obj.Entradas);
                    cmd.Parameters.AddWithValue("Salidas", obj.Salidas);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("IdUsuarioAutorizador", (object)obj.IdUsuarioAutorizador ?? DBNull.Value);

                    // Parámetros de salida del SP
                    SqlParameter pResultado = new SqlParameter("Resultado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMensaje = new SqlParameter("Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pResultado);
                    cmd.Parameters.Add(pMensaje);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    idGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                Mensaje = ex.Message;
            }
            return idGenerado;
        }
   
        public bool AnularMovimiento(int idMovimiento, string motivo, int idUsuarioAnulador, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ANULAR_MOVIMIENTO_RECALCULAR", con);
                    cmd.Parameters.AddWithValue("ID_MOVIMIENTO", idMovimiento);
                    cmd.Parameters.AddWithValue("MOTIVO_ANULADO", motivo);
                    cmd.Parameters.AddWithValue("IDUSUARIO_ANULADOR", idUsuarioAnulador);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    // Ejecutamos y leemos el mensaje de éxito del procedimiento
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            mensaje = dr["MENSAJE"].ToString();
                            resultado = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
        public bool AplicarRetorno(int idMovimiento, decimal monto, string motivo, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_APLICAR_RETORNO", con);
                    cmd.Parameters.AddWithValue("IdMovimiento", idMovimiento);
                    cmd.Parameters.AddWithValue("MontoRetorno", monto);
                    cmd.Parameters.AddWithValue("Motivo", motivo);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    // CAMBIO CLAVE: Usamos ExecuteReader para capturar el SELECT del SP
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Leemos las columnas 'Resultado' y 'Mensaje' que definimos en el SQL
                            resultado = Convert.ToInt32(dr["Resultado"]) == 1;
                            mensaje = dr["Mensaje"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = "Error en Capa Datos: " + ex.Message;
            }
            return resultado;
        }



    }
}
