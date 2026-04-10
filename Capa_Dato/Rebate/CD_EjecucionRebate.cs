using Capa_Entidad;
using Capa_Entidad.CE_Rebate;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.Rebate
{
    public class CD_EjecucionRebate
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<EjecucionRebate> ObtenerEjecuciones()
        {
            var lista = new List<EjecucionRebate>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_EJECUCIONES", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EjecucionRebate
                        {
                            IdEjecucion = Convert.ToInt32(dr["ID_EJECUCION"]),
                            IdAcuerdo = Convert.ToInt32(dr["ID_ACUERDO"]),
                            NumeroAcuerdo = dr["NUMERO_ACUERDO"].ToString(),
                            NombreAcuerdo = dr["NOMBRE_ACUERDO"].ToString(),

                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString(),

                            NumeroDocumentoSoporte = dr["NUMERO_DOCUMENTO_SOPORTE"].ToString(),
                            MontoCalculado = dr["MONTO_CALCULADO"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(dr["MONTO_CALCULADO"]),
                            MontoRebate = dr["MONTO_REBATE"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(dr["MONTO_REBATE"]),

                            CumpleCondicion = dr["CUMPLE_CONDICION"] == DBNull.Value
                            ? false
                            : Convert.ToBoolean(dr["CUMPLE_CONDICION"]),

                            ArchivoSoporte = dr["ARCHIVO_SOPORTE"] == DBNull.Value
                            ? null
                            : dr["ARCHIVO_SOPORTE"].ToString(),
                            // NUEVO: Criterio y Valor
                            Criterio = dr["CRITERIO"].ToString(),
                            ValorCriterio = Convert.ToDecimal(dr["VALOR_CRITERIO"]),

                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            EstadoNombre = dr["ESTADO"].ToString(),

                            FechaPago = Convert.ToDateTime(dr["FECHA_PAGO"]),
                            UsuarioRegistro = dr["USUARIO_REGISTRO"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                            UsuarioModificacion = dr["USUARIO_MODIFICACION"] == DBNull.Value
                            ? null
                            : dr["USUARIO_MODIFICACION"].ToString(),
                            FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value
                            ? null
                            : Convert.ToDateTime(dr["FECHA_MODIFICACION"])
                        });

                    }
                }
            }

            return lista;
        }

        public bool CrearEjecucion(EjecucionRebate obj, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_EJECUCION_REBATE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_ACUERDO", obj.IdAcuerdo);
                    cmd.Parameters.AddWithValue("@MONTO_CALCULADO", obj.MontoCalculado);
                    cmd.Parameters.AddWithValue(
                         "@ARCHIVO_SOPORTE",
                         (object)obj.ArchivoSoporte ?? DBNull.Value
                     );
                    cmd.Parameters.AddWithValue(
                        "@USUARIO_REGISTRO",
                        (object)obj.UsuarioRegistro ?? DBNull.Value
                    );
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool EditarEjecucion(EjecucionRebate obj, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_EJECUCION_REBATE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_EJECUCION", obj.IdEjecucion);
                    cmd.Parameters.AddWithValue("@MONTO_CALCULADO", obj.MontoCalculado);
                    cmd.Parameters.AddWithValue("@ARCHIVO_SOPORTE", (object)obj.ArchivoSoporte ?? DBNull.Value);


                    cmd.Parameters.AddWithValue("@USUARIO_MODIFICACION", obj.UsuarioModificacion);


                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public List<EjecucionRebate> ObtenerEjecucionesRebateResumen()
        {
            var lista = new List<EjecucionRebate>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LISTA_EJECUCIONES_REBATE_RESUMEN", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new EjecucionRebate
                            {
                                IdEjecucion = Convert.ToInt32(dr["ID_EJECUCION"]),
                                NumeroDocumentoSoporte = dr["NUMERO_DOCUMENTO_SOPORTE"].ToString(),

                                NumeroAcuerdo = dr["NUMERO_ACUERDO"].ToString(),
                                NombreAcuerdo = dr["NOMBRE_ACUERDO"].ToString(),

                                NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString(),
                                TipoRebate = dr["TIPO_REBATE"].ToString(),
                                Criterio = dr["CRITERIO"].ToString(),

                                Saldovencido = Convert.ToDecimal(dr["SALDO_VENCIDO"]),
                                MontoRebate = Convert.ToDecimal(dr["MONTO_REBATE"]),
                                TotalRetenciones = Convert.ToDecimal(dr["TOTAL_RETENCIONES"]),
                                TotalNeto = Convert.ToDecimal(dr["TOTAL_NETO"]),
                                MontoPagado = Convert.ToDecimal(dr["MONTO_PAGADO"]),
                                SaldoPendiente = Convert.ToDecimal(dr["SALDO_PENDIENTE"]),

                                IdEstado = Convert.ToInt32(dr["ID_ESTADO_EJECUCION"]),
                                EstadoNombre = dr["ESTADO_EJECUCION"].ToString(),


                                FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<DetallePagoEjecucionRebate> ObtenerDetallePagoEjecucionRebate(int idEjecucion)
        {
            var lista = new List<DetallePagoEjecucionRebate>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DETALLE_PAGO_EJECUCION_REBATE", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_EJECUCION", idEjecucion);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetallePagoEjecucionRebate
                            {
                                IdDetallePago = dr["IdDetallePago"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdDetallePago"]),
                                DocumentoPago = dr["DocumentoPago"]?.ToString(),
                                MontoPagado = dr["MontoPagado"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MontoPagado"]),
                                TipoDocumento = dr["TipoDocumento"]?.ToString(),
                                NumeroConfirmacion = dr["NUMERO_CONFIRMACION"] == DBNull.Value ? "No disponible" : dr["NUMERO_CONFIRMACION"].ToString(),
                                usuarioPago = dr["usuarioPago"]?.ToString(),
                                FechaRegistro = dr["FechaRegistro"] == DBNull.Value
                                ? DateTime.MinValue
                                : Convert.ToDateTime(dr["FechaRegistro"]),
                                Comprobante = dr["Comprobante"]?.ToString(),
                                IdEstado = Convert.ToInt32(dr["IdEstado"]),
                                Observacion = dr["Observacion"] == DBNull.Value ? null : dr["Observacion"].ToString(),
                                NotaCreditoAplicada = Convert.ToBoolean(dr["NotaCreditoAplicada"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public bool AnularPagoEjecucionRebate(int idDetallePago, string usuario)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ANULAR_PAGO_EJECUCION_REBATE", cn))
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
            catch (SqlException)
            {
                resultado = false;
            }

            return resultado;
        }

        //ITEM

        public List<EjecucionRebate> ObtenerEjecucionesItem()
        {
            var lista = new List<EjecucionRebate>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_EJECUCIONES_ITEM", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EjecucionRebate
                        {
                            IdEjecucion = Convert.ToInt32(dr["ID_EJECUCION"]),
                            IdAcuerdo = Convert.ToInt32(dr["ID_ACUERDO"]),

                            NumeroAcuerdo = dr["NUMERO_ACUERDO"]?.ToString(),
                            NombreAcuerdo = dr["NOMBRE_ACUERDO"]?.ToString(),

                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"]?.ToString(),
                            Ganancia = Convert.ToDecimal(dr["GANANCIA"]),
                            NumeroDocumentoSoporte = dr["NUMERO_DOCUMENTO_SOPORTE"]?.ToString(),

                            MontoCalculado = dr["MONTO_CALCULADO"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(dr["MONTO_CALCULADO"]),
                            MontoRebate = dr["MONTO_REBATE"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MONTO_REBATE"]),
                            CumpleCondicion = dr["CUMPLE_CONDICION"] == DBNull.Value ? false : Convert.ToBoolean(dr["CUMPLE_CONDICION"]),

                            ArchivoSoporte = dr["ARCHIVO_SOPORTE"]?.ToString(),

                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            EstadoNombre = dr["ESTADO"]?.ToString(),

                            Comentario = dr["COMENTARIO"] == DBNull.Value
                            ? null
                            : dr["COMENTARIO"].ToString(),

                                                    Forzado = dr["FORZADO"] == DBNull.Value
                            ? false
                            : Convert.ToBoolean(dr["FORZADO"]),


                            FechaPago = dr["FECHA_PAGO"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["FECHA_PAGO"]),
                            UsuarioRegistro = dr["USUARIO_REGISTRO"]?.ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),

                            UsuarioModificacion = dr["USUARIO_MODIFICACION"] == DBNull.Value
                                ? null
                                : dr["USUARIO_MODIFICACION"].ToString(),

                            FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(dr["FECHA_MODIFICACION"]),

                            // 🔥 NUEVO: Departamento
                            NombreDepartamento = dr["NOMBRE_DEPARTAMENTO"] == DBNull.Value
                                ? null
                                : dr["NOMBRE_DEPARTAMENTO"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public bool CrearEjecucionItemCompleto(EjecucionRebate obj, string detallesJson, out int idEjecucion, out string mensaje)
        {
            idEjecucion = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_EJECUCION_REBATE_ITEM_COMPLETO", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_ACUERDO", obj.IdAcuerdo);
                    cmd.Parameters.AddWithValue("@ARCHIVO_SOPORTE", (object)obj.ArchivoSoporte ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MONTO_CALCULADO", (object)obj.MontoCalculado ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MONTO_REBATE", (object)obj.MontoRebate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CUMPLE_CONDICION", (object)obj.CumpleCondicion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@COMENTARIO", (object)obj.Comentario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FORZADO", obj.Forzado);
                    cmd.Parameters.AddWithValue("@USUARIO_REGISTRO", obj.UsuarioRegistro);
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);
                    cmd.Parameters.AddWithValue("@DETALLES", detallesJson);

                    cn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        idEjecucion = Convert.ToInt32(result);
                }

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool EditarEjecucionItem(EjecucionRebate obj, string detallesJson, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_EJECUCION_REBATE_ITEM_COMPLETO", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_EJECUCION", obj.IdEjecucion);
                    cmd.Parameters.AddWithValue("@ARCHIVO_SOPORTE", (object)obj.ArchivoSoporte ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MONTO_CALCULADO", (object)obj.MontoCalculado ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MONTO_REBATE", (object)obj.MontoRebate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CUMPLE_CONDICION", (object)obj.CumpleCondicion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@USUARIO_MODIFICACION", obj.UsuarioModificacion);
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);
                    cmd.Parameters.AddWithValue("@COMENTARIO", (object)obj.Comentario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FORZADO", obj.Forzado);
                    cmd.Parameters.AddWithValue("@DETALLES", detallesJson);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }
        public List<DetalleAcuerdo> ObtenerDetalleEjecucion(int idEjecucion)
        {
            var lista = new List<DetalleAcuerdo>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_DETALLE_EJECUCION", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_EJECUCION", idEjecucion);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DetalleAcuerdo
                        {
                            IdDetalle = Convert.ToInt32(dr["ID_DETALLE"]),
                            Descripcion = dr["DESCRIPCION"].ToString(),
                            Cantidad = Convert.ToDecimal(dr["CANTIDAD_BASE"]),
                            CantidadIngresada = Convert.ToDecimal(dr["CANTIDAD_INGRESADA"])
                        });
                    }
                }
            }

            return lista;
        }

        public List<EjecucionRebate> ObtenerEjecucionesDescuento()
        {
            var lista = new List<EjecucionRebate>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_EJECUCIONES_DESCUENTO", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EjecucionRebate
                        {
                            IdEjecucion = Convert.ToInt32(dr["ID_EJECUCION"]),
                            IdAcuerdo = Convert.ToInt32(dr["ID_ACUERDO"]),

                            NumeroAcuerdo = dr["NUMERO_ACUERDO"]?.ToString(),
                            NombreAcuerdo = dr["NOMBRE_ACUERDO"]?.ToString(),

                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"]?.ToString(),
                            NumeroDocumentoSoporte = dr["NUMERO_DOCUMENTO_SOPORTE"]?.ToString(),

                            MontoRebate = dr["MONTO_REBATE"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MONTO_REBATE"]),
                            CumpleCondicion = dr["CUMPLE_CONDICION"] == DBNull.Value ? false : Convert.ToBoolean(dr["CUMPLE_CONDICION"]),

                            ArchivoSoporte = dr["ARCHIVO_SOPORTE"]?.ToString(),

                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            EstadoNombre = dr["ESTADO"]?.ToString(),

                            Comentario = dr["COMENTARIO"] == DBNull.Value
                            ? null
                            : dr["COMENTARIO"].ToString(),

                            FechaPago = dr["FECHA_PAGO"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["FECHA_PAGO"]),
                            UsuarioRegistro = dr["USUARIO_REGISTRO"]?.ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),

                            UsuarioModificacion = dr["USUARIO_MODIFICACION"] == DBNull.Value
                                ? null
                                : dr["USUARIO_MODIFICACION"].ToString(),

                            FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(dr["FECHA_MODIFICACION"]),

                            // 🔥 NUEVO: Departamento
                            NombreDepartamento = dr["NOMBRE_DEPARTAMENTO"] == DBNull.Value
                                ? null
                                : dr["NOMBRE_DEPARTAMENTO"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<DetalleAcuerdo> ObtenerDetalleEjecucionDescuento(int idEjecucion)
        {
            var lista = new List<DetalleAcuerdo>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_DETALLE_EJECUCION_DESCUENTO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_EJECUCION", idEjecucion);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DetalleAcuerdo
                        {
                            IdDetalle = Convert.ToInt32(dr["ID_DETALLE"]),
                            Descripcion = dr["DESCRIPCION"].ToString(),
                            Porcentaje = Convert.ToDecimal(dr["PORCENTAJE"]),
                            PrecioBase = Convert.ToDecimal(dr["PRECIO_BASE"]),
                            CantidadIngresada = Convert.ToDecimal(dr["CANTIDAD_INGRESADA"])
                        });
                    }
                }
            }

            return lista;
        }
    }
}
