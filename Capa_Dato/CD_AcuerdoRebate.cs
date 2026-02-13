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
                        Documento = dr["DOCUMENTO"].ToString(),
                        oProveedor = new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
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

        // ============================
        // CREAR ACUERDO REBATE
        // ============================
        public bool CrearAcuerdo(AcuerdoRebate obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_ACUERDO_REBATE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE_ACUERDO", obj.NombreAcuerdo);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD_OP", obj.IdModalidadOp);
                    cmd.Parameters.AddWithValue("@ID_TIPO_REBATE", obj.IdTipoRebate);
                    cmd.Parameters.AddWithValue("@ID_CRITERIO", obj.IdCriterio);
                    cmd.Parameters.AddWithValue("@VALOR_CRITERIO", obj.ValorCriterio);
                    cmd.Parameters.AddWithValue("@GANANCIA", obj.Ganancia);
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.AddWithValue("@FECHA_INICIO", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FECHA_FIN", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? string.Empty);
                    cmd.Parameters.AddWithValue("@USUARIO_CREACION", obj.UsuarioCreacion); // obligatorio
                    cmd.Parameters.AddWithValue("@DOCUMENTO", obj.Documento ?? string.Empty); // opcional

                    cn.Open();
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

        // ============================
        // EDITAR ACUERDO REBATE
        // ============================
        public bool EditarAcuerdo(AcuerdoRebate obj, out string mensaje)
        {
            mensaje = string.Empty;
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
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD_OP", obj.IdModalidadOp);
                    cmd.Parameters.AddWithValue("@ID_TIPO_REBATE", obj.IdTipoRebate);
                    cmd.Parameters.AddWithValue("@ID_CRITERIO", obj.IdCriterio);
                    cmd.Parameters.AddWithValue("@VALOR_CRITERIO", obj.ValorCriterio);
                    cmd.Parameters.AddWithValue("@GANANCIA", obj.Ganancia);
                    cmd.Parameters.AddWithValue("@ID_PAIS", obj.IdPais);
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.AddWithValue("@FECHA_INICIO", obj.FechaInicio);
                    cmd.Parameters.AddWithValue("@FECHA_FIN", obj.FechaFin);
                    cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? string.Empty);
                    cmd.Parameters.AddWithValue("@DOCUMENTO", obj.Documento ?? string.Empty); 

                    cn.Open();
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

    }
}
