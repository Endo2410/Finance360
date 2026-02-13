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
    public class CD_Vencidos
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Vencido> ObtenerVencidos(DateTime inicio, DateTime fin)
        {
            List<Vencido> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ORDENES_VENCIDAS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FECHA_INICIO", inicio);
                cmd.Parameters.AddWithValue("@FECHA_FIN", fin);

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Vencido
                        {
                            IdVencido = Convert.ToInt32(dr["IDVENCIDO"]),
                            IdOrdenVencido = Convert.ToInt32(dr["IDORDENVENCIDO"]),
                            HQID = Convert.ToInt32(dr["HQID"]),
                            Proveedor = dr["PROVEEDOR"].ToString(),
                            NumeroOrden = dr["NUMERO_ORDEN"].ToString(),
                            StatusOrden = dr["STATUS_ORDEN"].ToString(),
                            Concepto = dr["CONCEPTO"].ToString(),
                            FechaCreacion = Convert.ToDateTime(dr["FECHA_CREACION"]),
                            FechaVencimiento = Convert.ToDateTime(dr["FECHA_VENCIMIENTO"]),
                            Usuario = dr["USUARIO"].ToString(),
                            Total = Convert.ToDecimal(dr["TOTAL"]),
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            NombreEstado = dr["NOMBRE_ESTADO"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public int InsertarVencidosNuevos()
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_INSERTAR_ORDENES_VENCIDAS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
            }
        }

        public List<Vencido> ObtenerVencidosPendientes()
        {
            List<Vencido> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        V.IDVENCIDO,
                        V.NUMERO_ORDEN,
                        V.CONCEPTO,
                        V.FECHA_CREACION,
                        V.TOTAL,
                        P.NOMBRE_PROVEEDOR AS NOMBRE_PROVEEDOR
                    FROM VENCIDOS V
                    INNER JOIN PROVEEDORES P ON P.HQID = V.HQID
                       WHERE V.IDESTADO IN (5, 9)
                    ORDER BY V.FECHA_CREACION
                ", conn);

                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Vencido
                        {
                            IdVencido = (int)dr["IDVENCIDO"],
                            NumeroOrden = dr["NUMERO_ORDEN"].ToString(),
                            Concepto = dr["CONCEPTO"].ToString(),
                            FechaCreacion = (DateTime)dr["FECHA_CREACION"],
                            Total = (decimal)dr["TOTAL"],
                            Proveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<Vencido> ObtenerVencido1()
        {
            var lista = new List<Vencido>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LISTA_VENCIDOS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Vencido
                            {
                                IdVencido = Convert.ToInt32(dr["IDVENCIDO"]),
                                IdOrdenVencido = Convert.ToInt32(dr["IDORDENVENCIDO"]),
                                NumeroOrden = dr["NUMERO_ORDEN"].ToString(),
                                StatusOrden = dr["STATUS_ORDEN"].ToString(),
                                NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString(),
                                Concepto = dr["CONCEPTO"].ToString(),
                                FechaCreacion = Convert.ToDateTime(dr["FECHA_CREACION"]),
                                Usuario = dr["USUARIO"].ToString(),
                                Saldovencido = Convert.ToDecimal(dr["SALDO_VENCIDO"]),
                                MontoTotal = Convert.ToDecimal(dr["MONTO_TOTAL"]),
                                MontoPagado = Convert.ToDecimal(dr["MONTO_PAGADO"]),
                                SaldoPendiente = Convert.ToDecimal(dr["SALDO_PENDIENTE"]),
                                IdEstado = Convert.ToInt32(dr["ID_ESTADO_PAGO"]),
                                NombreEstado = dr["ESTADO_PAGO"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public List<DetallePagoVencido> ObtenerDetallePagoVencido(int idVencido)
        {
            var lista = new List<DetallePagoVencido>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DETALLE_PAGO_VENCIDO", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_VENCIDO", idVencido);
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetallePagoVencido
                            {
                                IdDetallePago = dr["IdDetallePago"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdDetallePago"]),
                                DocumentoPago = dr["DocumentoPago"] == DBNull.Value ? null : dr["DocumentoPago"].ToString(),
                                MontoPagado1 = dr["MontoPagado"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["MontoPagado"]),
                                TipoDocumento = dr["TipoDocumento"] == DBNull.Value ? null : dr["TipoDocumento"].ToString(),
                                NumeroConfirmacion = dr["NUMERO_CONFIRMACION"] == DBNull.Value ? "No disponible" : dr["NUMERO_CONFIRMACION"].ToString(), 
                                usuarioPago = dr["usuarioPago"]?.ToString(),
                                FechaDocumento = dr["FechaDocumento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaDocumento"]),
                                FechaRegistro = dr["FechaRegistro"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaRegistro"]),
                                Comprobante = dr["Comprobante"] == DBNull.Value ? null : dr["Comprobante"].ToString(),
                                IdEstado = Convert.ToInt32(dr["IdEstado"]),
                                NotaCreditoAplicada = Convert.ToBoolean(dr["NotaCreditoAplicada"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public bool AnularPagoVencido(int idDetallePago, string usuario)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_ANULAR_PAGO_VENCIDO", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_DETALLE_PAGO", idDetallePago);
                    cmd.Parameters.AddWithValue("@USUARIO_ANULA", usuario);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
