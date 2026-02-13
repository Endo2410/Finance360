using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Capa_Dato
{
    public class CD_CampaniaPublicitaria
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<CampaniaPublicitaria> ObtenerCampanias()
        {
            List<CampaniaPublicitaria> lista = new List<CampaniaPublicitaria>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_CAMPANIA_PUBLICITARIA", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CampaniaPublicitaria c = new CampaniaPublicitaria
                    {
                        IdCampania = Convert.ToInt32(dr["ID_CAMPANIA"]),
                        NumeroCampania = dr["NUMERO_CAMPANIA"]?.ToString(),
                        NombreCampania = dr["NOMBRE_CAMPANIA"]?.ToString(),
                        MontoInversion = Convert.ToDecimal(dr["MONTO_INVERSION"]),
                        FechaInicio = Convert.ToDateTime(dr["FECHA_INICIO"]),
                        FechaFin = Convert.ToDateTime(dr["FECHA_FIN"]),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        UsuarioRegistro = dr["USUARIO_REGISTRO"]?.ToString(),
                        DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"]?.ToString(),

                        IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                        IdTipoPublicidad = Convert.ToInt32(dr["ID_TIPO_PUBLICIDAD"]),
                        IdModalidad = Convert.ToInt32(dr["ID_MODALIDAD"]),
                        IdPais = Convert.ToInt32(dr["ID_PAIS"]),
                        IdMoneda = Convert.ToInt32(dr["ID_MONEDA"]),

                        // ESTADO GENERAL
                        IdEstado = Convert.ToInt32(dr["IDESTADO"]),

                        // ESTADO DE PAGO
                        IdEstadoPago = Convert.ToInt32(dr["IDESTADO_PAGO"]),

                        oProveedor = new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                        },

                        oTipoPublicidad = new TipoPublicidad
                        {
                            IdTipoPublicidad = Convert.ToInt32(dr["ID_TIPO_PUBLICIDAD"]),
                            Nombre = dr["TIPO_PUBLICIDAD"].ToString()
                        },

                        oModalidad = new Modalidad
                        {
                            IdModalidad = Convert.ToInt32(dr["ID_MODALIDAD"]),
                            Nombre = dr["MODALIDAD"].ToString()
                        },

                        oPais = new Pais
                        {
                            IdPais = Convert.ToInt32(dr["ID_PAIS"]),
                            Nombre = dr["PAIS"].ToString()
                        },

                        oMoneda = new Moneda
                        {
                            IdMoneda = Convert.ToInt32(dr["ID_MONEDA"]),
                            Nombre = dr["MONEDA"].ToString()
                        },

                        // ESTADO GENERAL
                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["ESTADO"].ToString()
                        },

                        // ESTADO PAGO
                        oEstadoPago = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO_PAGO"]),
                            Nombre = dr["ESTADO_PAGO"].ToString()
                        }
                    };

                    lista.Add(c);
                }
            }

            return lista;
        }


        public bool CrearCampania(CampaniaPublicitaria obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_CAMPANIA_PUBLICITARIA", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE_CAMPANIA", obj.NombreCampania);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_TIPO_PUBLICIDAD", obj.IdTipoPublicidad);
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD", obj.IdModalidad);
                    cmd.Parameters.AddWithValue("@FECHA_INICIO", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FECHA_FIN", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@MONTO_INVERSION", obj.MontoInversion);
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.AddWithValue("@USUARIO_REGISTRO", obj.UsuarioRegistro);
                    cmd.Parameters.Add("@DOCUMENTO_ADJUNTO", SqlDbType.VarChar, 500)
                   .Value = string.IsNullOrEmpty(obj.DocumentoAdjunto)
                            ? DBNull.Value
                            : obj.DocumentoAdjunto;


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

        public bool EditarCampania(CampaniaPublicitaria obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_CAMPANIA_PUBLICITARIA", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_CAMPANIA", obj.IdCampania);
                    cmd.Parameters.AddWithValue("@NOMBRE_CAMPANIA", obj.NombreCampania);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_TIPO_PUBLICIDAD", obj.IdTipoPublicidad);
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD", obj.IdModalidad);
                    cmd.Parameters.AddWithValue("@FECHA_INICIO", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FECHA_FIN", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@MONTO_INVERSION", obj.MontoInversion);
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.Add("@DOCUMENTO_ADJUNTO", SqlDbType.VarChar, 500)
                    .Value = string.IsNullOrEmpty(obj.DocumentoAdjunto)
                             ? DBNull.Value
                             : obj.DocumentoAdjunto;

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

        public List<CampaniaPublicitaria> ObtenerCampaniasResumen()
        {
            List<CampaniaPublicitaria> lista = new List<CampaniaPublicitaria>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTA_CAMPANIAS_PUBLICIDAD", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    CampaniaPublicitaria c = new CampaniaPublicitaria
                    {
                        IdCampania = Convert.ToInt32(dr["ID_CAMPANIA"]),
                        NumeroCampania = dr["NUMERO_CAMPANIA"].ToString(),
                        NombreCampania = dr["NOMBRE_CAMPANIA"].ToString(),
                        MontoInversion = Convert.ToDecimal(dr["MONTO_INVERSION"]),
                        Saldovencido = Convert.ToDecimal(dr["SALDO_VENCIDO"]),
                        MontoPagado = Convert.ToDecimal(dr["MONTO_PAGADO"]),
                        SaldoPendiente = Convert.ToDecimal(dr["SALDO_PENDIENTE"]),
                        EstadoCampania = dr["ESTADO_CAMPANIA"].ToString(),
                        IdEstado = Convert.ToInt32(dr["ID_ESTADO_PAGO"]), //  CLAVE
                        EstadoPago = dr["ESTADO_PAGO"].ToString(),
                        FechaInicio = Convert.ToDateTime(dr["FECHA_INICIO"]),
                        FechaFin = Convert.ToDateTime(dr["FECHA_FIN"]),
                        oProveedor = new Proveedor { NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString() },
                        oTipoPublicidad = new TipoPublicidad { Nombre = dr["TIPO_PUBLICIDAD"].ToString() },
                        oModalidad = new Modalidad { Nombre = dr["MODALIDAD"].ToString() },
                        oPais = new Pais { Nombre = dr["PAIS"].ToString() },
                        oMoneda = new Moneda { Nombre = dr["MONEDA"].ToString() }
                    };
                    lista.Add(c);
                }
            }

            return lista;
        }

        public List<DetallePagoCampania> ObtenerDetallePagoCampania(int idCampania)
        {
            var lista = new List<DetallePagoCampania>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DETALLE_PAGO_CAMPANIA", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_CAMPANIA", idCampania);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetallePagoCampania
                            {
                                IdDetallePago = dr["IdDetallePago"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdDetallePago"]),
                                DocumentoPago = dr["DocumentoPago"] == DBNull.Value ? null : dr["DocumentoPago"].ToString(),
                                MontoPagado = dr["MontoPagado"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["MontoPagado"]),
                                TipoDocumento = dr["TipoDocumento"] == DBNull.Value ? null : dr["TipoDocumento"].ToString(),
                                NumeroConfirmacion = dr["NUMERO_CONFIRMACION"] == DBNull.Value ? "No disponible" : dr["NUMERO_CONFIRMACION"].ToString(), // <-- agregado
                                usuarioPago = dr["usuarioPago"]?.ToString(),
                                FechaDocumento = dr["FechaDocumento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaDocumento"]),
                                FechaRegistro = dr["FechaRegistro"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaRegistro"]),
                                Comprobante = dr["Comprobante"] == DBNull.Value ? null : dr["Comprobante"].ToString(),
                                IdEstado = dr["IdEstado"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdEstado"]),
                                NotaCreditoAplicada = dr["NotaCreditoAplicada"] != DBNull.Value && Convert.ToBoolean(dr["NotaCreditoAplicada"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public bool AnularPagoPublicidad(int idDetallePago, string usuario)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ANULAR_PAGO_PUBLICIDAD", cn))
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

    }
}
