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
    public class CD_AcuerdoRebate
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<AcuerdoRebate> ObtenerAcuerdos()
        {
            var lista = new List<AcuerdoRebate>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ACUERDO_REBATE", cn);


                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new AcuerdoRebate
                    {
                        IdAcuerdo = Convert.ToInt32(dr["ID_ACUERDO"]),
                        NumeroAcuerdo = dr["NUMERO_ACUERDO"].ToString(),
                        NombreAcuerdo = dr["NOMBRE_ACUERDO"].ToString(),
                        ValorCriterio = Convert.ToDecimal(dr["VALOR_CRITERIO"]),
                        Ganancia = Convert.ToDecimal(dr["GANANCIA"]),
                        FechaInicio = Convert.ToDateTime(dr["FECHA_INICIO"]),
                        FechaFin = Convert.ToDateTime(dr["FECHA_FIN"]),
                        Comentario = dr["COMENTARIO"].ToString(),
                        UsuarioCreacion = dr["USUARIO_CREACION"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        UsuarioModificacion = dr["USUARIO_MODIFICACION"] == DBNull.Value
                        ? null
                        : dr["USUARIO_MODIFICACION"].ToString(),

                        FechaModificacion = dr["FECHA_MODIFICACION"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(dr["FECHA_MODIFICACION"]),
                        Documento = dr["DOCUMENTO"].ToString(),
                        oProveedor = new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                        },
                        ODepartamento = dr["ID_DEPARTAMENTO"] == DBNull.Value
                        ? null
                        : new Departamento
                        {
                            IdDepartamento = Convert.ToInt32(dr["ID_DEPARTAMENTO"]),
                            NombreDepartamento = dr["DEPARTAMENTO"]?.ToString()
                        },
                        oModalidadOperacion = new ModalidadOperacion
                        {
                            IdModalidadOp = Convert.ToInt32(dr["ID_MODALIDAD_OP"]),
                            Nombre = dr["MODALIDAD_OP"].ToString()
                        },
                        oTipoRebate = new TipoRebate
                        {
                            IdTipoRebate = Convert.ToInt32(dr["ID_TIPO_REBATE"]),
                            Nombre = dr["TIPO_REBATE"].ToString()
                        },
                        oCriterio = new CriterioRebate
                        {
                            IdCriterio = Convert.ToInt32(dr["ID_CRITERIO"]),
                            Nombre = dr["CRITERIO"].ToString()
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

        public List<DetalleAcuerdo> ObtenerDetalles(int idAcuerdo)
        {
            var lista = new List<DetalleAcuerdo>();
            using SqlConnection cn = new SqlConnection(cadenaConexion);
            using SqlCommand cmd = new SqlCommand("SP_OBTENER_DETALLES_ACUERDO", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ID_ACUERDO", idAcuerdo);
            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new DetalleAcuerdo
                {
                    IdDetalle = Convert.ToInt32(dr["ID_DETALLE"]),
                    IdAcuerdo = Convert.ToInt32(dr["ID_ACUERDO"]),
                    Descripcion = dr["DESCRIPCION"].ToString(),
                    Cantidad = dr["CANTIDAD_OBJETIVO"] == DBNull.Value ? null : Convert.ToDecimal(dr["CANTIDAD_OBJETIVO"]),
                    Porcentaje = dr["PORCENTAJE"] == DBNull.Value ? null : Convert.ToDecimal(dr["PORCENTAJE"]),
                    PrecioBase = dr["PRECIO_BASE"] == DBNull.Value ? null : Convert.ToDecimal(dr["PRECIO_BASE"])
                });
            }
            return lista;
        }

        // ============================
        // CREAR ACUERDO REBATE
        // ============================
        public bool CrearAcuerdo(AcuerdoRebate obj, List<DetalleAcuerdo> detalles, out string mensaje)
        {
            mensaje = "";
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_ACUERDO_REBATE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Campos normales
                    cmd.Parameters.AddWithValue("@NOMBRE_ACUERDO", obj.NombreAcuerdo);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_DEPARTAMENTO", obj.IdDepartamento);
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD_OP", obj.IdModalidadOp);
                    cmd.Parameters.AddWithValue("@ID_TIPO_REBATE", obj.IdTipoRebate);
                    cmd.Parameters.AddWithValue("@ID_CRITERIO", obj.IdCriterio);
                    cmd.Parameters.AddWithValue("@VALOR_CRITERIO", obj.ValorCriterio);
                    cmd.Parameters.AddWithValue("@GANANCIA", obj.Ganancia);
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.AddWithValue("@FECHA_INICIO", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FECHA_FIN", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? "");
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", obj.UsuarioCreacion);
                    cmd.Parameters.AddWithValue("@DOCUMENTO", obj.Documento ?? "");

                    // 🔥 TABLA DETALLE
                    DataTable dt = new DataTable();
                    dt.Columns.Add("DESCRIPCION", typeof(string));
                    dt.Columns.Add("CANTIDAD", typeof(decimal));
                    dt.Columns.Add("PORCENTAJE", typeof(decimal));
                    dt.Columns.Add("PRECIO_BASE", typeof(decimal));

                    foreach (var item in detalles)
                    {
                        dt.Rows.Add(item.Descripcion,
                            item.Cantidad ?? (object)DBNull.Value, 
                            item.Porcentaje ?? (object)DBNull.Value,
                            item.PrecioBase ?? (object)DBNull.Value
                        );
                    }

                    SqlParameter p = cmd.Parameters.AddWithValue("@DETALLE", dt);
                    p.SqlDbType = SqlDbType.Structured;
                    p.TypeName = "TIPO_DETALLE_ACUERDO";

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }

        // ============================
        // EDITAR ACUERDO REBATE
        // ============================
        public bool EditarAcuerdo(AcuerdoRebate obj, List<DetalleAcuerdo> detalles, out string mensaje)
        {
            mensaje = "";
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_ACUERDO_REBATE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_ACUERDO", obj.IdAcuerdo);
                    cmd.Parameters.AddWithValue("@NOMBRE_ACUERDO", obj.NombreAcuerdo);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_DEPARTAMENTO", obj.IdDepartamento);
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD_OP", obj.IdModalidadOp);
                    cmd.Parameters.AddWithValue("@ID_TIPO_REBATE", obj.IdTipoRebate);
                    cmd.Parameters.AddWithValue("@ID_CRITERIO", obj.IdCriterio);
                    cmd.Parameters.AddWithValue("@VALOR_CRITERIO", obj.ValorCriterio);
                    cmd.Parameters.AddWithValue("@GANANCIA", obj.Ganancia);
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.AddWithValue("@FECHA_INICIO", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FECHA_FIN", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? "");
                    cmd.Parameters.AddWithValue("@DOCUMENTO", obj.Documento ?? "");
                    cmd.Parameters.AddWithValue("@USUARIO_MODIFICACION", obj.UsuarioModificacion);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    // 🔥 Editar detalles: primero eliminamos existentes y luego insertamos
                    if (detalles != null && detalles.Any())
                    {
                        SqlCommand cmdDelete = new SqlCommand("DELETE FROM DETALLE_ACUERDO_REBATE WHERE ID_ACUERDO = @ID_ACUERDO", cn);
                        cmdDelete.Parameters.AddWithValue("@ID_ACUERDO", obj.IdAcuerdo);
                        cmdDelete.ExecuteNonQuery();

                        DataTable dt = new DataTable();
                        dt.Columns.Add("DESCRIPCION", typeof(string));
                        dt.Columns.Add("CANTIDAD", typeof(decimal));
                        dt.Columns.Add("PORCENTAJE", typeof(decimal));
                        dt.Columns.Add("PRECIO_BASE", typeof(decimal));

                        foreach (var item in detalles)
                        {
                            dt.Rows.Add(
                                item.Descripcion,
                                item.Cantidad ?? (object)DBNull.Value,
                                item.Porcentaje ?? (object)DBNull.Value,
                                item.PrecioBase ?? (object)DBNull.Value
                            );
                        }

                        SqlCommand cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLE_ACUERDO_REBATE", cn);
                        cmdDetalle.CommandType = CommandType.StoredProcedure;

                        SqlParameter p = cmdDetalle.Parameters.AddWithValue("@ID_ACUERDO", obj.IdAcuerdo);
                        SqlParameter dtParam = cmdDetalle.Parameters.AddWithValue("@DETALLE", dt);
                        dtParam.SqlDbType = SqlDbType.Structured;
                        dtParam.TypeName = "TIPO_DETALLE_ACUERDO";

                        cmdDetalle.ExecuteNonQuery();
                    }

                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }

    }
}
