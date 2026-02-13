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
    public class CD_Canje
    {
        private readonly string cn = Conexion.cn;
        private readonly string cadenaConexion = Conexion.cn;

        public List<Canje> ObtenerCanjes()
        {
            var lista = new List<Canje>();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_CANJE", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Canje
                    {
                        IdCanje = Convert.ToInt32(dr["ID_CANJE"]),
                        NumeroCanje = dr["NUMERO_CANJE"].ToString(),
                        Volumen = Convert.ToDecimal(dr["VOLUMEN"]),
                        Monto = Convert.ToDecimal(dr["MONTO"]),
                        FechaVencimiento = dr["FECHA_VENCIMIENTO"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(dr["FECHA_VENCIMIENTO"]),

                        UsuarioRegistro = dr["USUARIO_REGISTRO"].ToString(),
                        DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"].ToString(),
                        Comentario = dr["COMENTARIO"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        oProveedor = new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                        },
                        oTipoCanje = new TipoCanje
                        {
                            IdTipoCanje = Convert.ToInt32(dr["ID_TIPO_CANJE"]),
                            Nombre = dr["TIPO_CANJE"].ToString()
                        },
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

        public bool Crear(Canje obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using (SqlConnection con = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_CANJE", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_TIPO_CANJE", obj.IdTipoCanje);
                    cmd.Parameters.AddWithValue("@VOLUMEN", obj.Volumen);
                    cmd.Parameters.AddWithValue("@MONTO", obj.Monto);
                    cmd.Parameters.AddWithValue("@FECHA_VENCIMIENTO",
                    (object)obj.FechaVencimiento ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@USUARIO_REGISTRO", obj.UsuarioRegistro);
                    cmd.Parameters.AddWithValue("@DOCUMENTO_ADJUNTO", obj.DocumentoAdjunto ?? "");
                    cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool Editar(Canje obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using (SqlConnection con = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_CANJE", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_CANJE", obj.IdCanje);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_TIPO_CANJE", obj.IdTipoCanje);
                    cmd.Parameters.AddWithValue("@VOLUMEN", obj.Volumen);
                    cmd.Parameters.AddWithValue("@MONTO", obj.Monto);
                    cmd.Parameters.AddWithValue("@FECHA_VENCIMIENTO",
                    (object)obj.FechaVencimiento ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@DOCUMENTO_ADJUNTO", obj.DocumentoAdjunto ?? "");
                    cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public List<Canje> ObtenerCanjesresumen ()
        {
            var lista = new List<Canje>();
            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTA_CANJES", con); // SP resumen
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Canje
                    {
                        IdCanje = Convert.ToInt32(dr["ID_CANJE"]),
                        NumeroCanje = dr["NUMERO_CANJE"].ToString(),
                        Volumen = Convert.ToDecimal(dr["VOLUMEN"]),
                        Saldovencido = Convert.ToDecimal(dr["SALDO_VENCIDO"]),
                        Monto = Convert.ToDecimal(dr["MONTO"]),
                        MontoPagado = Convert.ToDecimal(dr["MONTO_PAGADO"]),
                        SaldoPendiente = Convert.ToDecimal(dr["SALDO_PENDIENTE"]),
                        FechaVencimiento = dr["FECHA_VENCIMIENTO"] == DBNull.Value
                            ? null
                            : Convert.ToDateTime(dr["FECHA_VENCIMIENTO"]),
                        UsuarioRegistro = dr["USUARIO_REGISTRO"].ToString(),
                        DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"].ToString(),
                        Comentario = dr["COMENTARIO"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        oProveedor = new Proveedor
                        {
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                        },
                        oTipoCanje = new TipoCanje
                        {
                            Nombre = dr["TIPO_CANJE"].ToString()
                        },
                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["ID_ESTADO"]),
                            Nombre = dr["ESTADO_CANJE"].ToString()
                        }
                    });
                }
            }
            return lista;
        }

        // 2️⃣ Obtener detalle de pagos de un canje
        public List<DetallePagoCanje1> ObtenerDetallePagoCanje(int idCanje)
        {
            var lista = new List<DetallePagoCanje1>();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_DETALLE_PAGO_CANJE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_CANJE", idCanje);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new DetallePagoCanje1
                    {
                        IdDetallePago = Convert.ToInt32(dr["IdDetallePago"]),
                        DocumentoPago = dr["DocumentoPago"].ToString(),
                        MontoPagado = Convert.ToDecimal(dr["MontoPagado"]),
                        TipoDocumento = dr["TipoDocumento"] == DBNull.Value ? "No disponible" : dr["TipoDocumento"].ToString(),
                        NumeroConfirmacion = dr["NUMERO_CONFIRMACION"] == DBNull.Value ? "" : dr["NUMERO_CONFIRMACION"].ToString(),
                        UsuarioPago = dr["UsuarioPago"] == DBNull.Value ? "UsuarioDesconocido" : dr["UsuarioPago"].ToString(),
                        Comprobante = dr["COMPROBANTE"] == DBNull.Value ? null : dr["COMPROBANTE"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        IdEstado = dr["IdEstado"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdEstado"]),
                        NotaCreditoAplicada = Convert.ToBoolean(dr["NotaCreditoAplicada"])
                    });
                }
            }

            return lista;
        }


        public bool AnularPagoCanje(int idDetallePago, string usuario)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ANULAR_PAGO_CANJE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_DETALLE_PAGO", idDetallePago);
                        cmd.Parameters.AddWithValue("@USUARIO_ANULA", usuario);

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        resultado = true;
                    }
                }
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }
    }
}
