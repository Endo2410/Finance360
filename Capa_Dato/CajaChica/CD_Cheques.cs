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
    public class CD_Cheques
    {
        public List<Cheque> Listar()
        {
            List<Cheque> lista = new List<Cheque>();

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    // Consulta directa con JOIN para obtener el nombre del usuario
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT C.ID_CHEQUE, C.NUMERO_CHEQUE, C.CONCEPTO, C.ENTRADA, C.IDUSUARIO,");
                    query.AppendLine("U.NOMBRES + ' ' + U.APELLIDOS AS USUARIO_REGISTRO,");
                    query.AppendLine("C.FECHA_REGISTRO, C.FOTO");
                    query.AppendLine("FROM CHEQUES C");
                    query.AppendLine("INNER JOIN USUARIO U ON C.IDUSUARIO = U.IDUSUARIO");
                    query.AppendLine("ORDER BY C.ID_CHEQUE DESC");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cheque()
                            {
                                // Asegúrate de tener estas propiedades en tu Entidad Cheque
                                NumeroCheque = dr["NUMERO_CHEQUE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NUMERO_CHEQUE"]),
                                Concepto = dr["CONCEPTO"] == DBNull.Value ? "" : dr["CONCEPTO"].ToString(),
                                Entrada = dr["ENTRADA"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["ENTRADA"]),
                                IdUsuario = dr["IDUSUARIO"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IDUSUARIO"]),

                                // Si agregaste esta propiedad a la entidad para mostrar el nombre
                                // UsuarioRegistro = dr["USUARIO_REGISTRO"] == DBNull.Value ? "N/A" : dr["USUARIO_REGISTRO"].ToString(),

                                FechaRegistro = dr["FECHA_REGISTRO"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["FECHA_REGISTRO"]),

                                // Manejo de la foto (arreglo de bytes)
                                Foto = dr["FOTO"] == DBNull.Value ? null : (byte[])dr["FOTO"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, devolvemos lista vacía
                lista = new List<Cheque>();
            }

            return lista;
        }
        public bool Registrar(Cheque obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERTAR_CHEQUE", con);
                    cmd.Parameters.AddWithValue("NUMERO_CHEQUE", obj.NumeroCheque);
                    cmd.Parameters.AddWithValue("CONCEPTO", obj.Concepto);
                    cmd.Parameters.AddWithValue("ENTRADA", obj.Entrada);
                    cmd.Parameters.AddWithValue("IDUSUARIO", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("FECHA_REGISTRO", obj.FechaRegistro);
                    // Si la foto es null, pasamos DBNull
                    cmd.Parameters.Add("FOTO", SqlDbType.VarBinary).Value = (object)obj.Foto ?? DBNull.Value;

                    // Parámetros de salida
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
