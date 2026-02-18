using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Entidad.Contabilidad_Alejandra;

namespace Capa_Dato
{
    public class CD_Incentivo
    {
        private readonly string cn = Conexion.cn;

        public List<Capa_Entidad.Incentivo> Obtener()
        {
            var lista = new List<Capa_Entidad.Incentivo>();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_INCENTIVO", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Capa_Entidad.Incentivo
                    {
                        IdIncentivo = Convert.ToInt32(dr["ID_INCENTIVO"]),
                        NumeroDocumento = dr["NUMERO_DOCUMENTO"].ToString(),
                        Nombre = dr["NOMBRE"].ToString(),
                        UsuarioRegistro = dr["USUARIO_REGISTRO"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"].ToString(),
                        Comentario = dr["COMENTARIO"].ToString(),

                        oSucursal = new E_Sucursales
                        {
                            IdSucursal = Convert.ToInt32(dr["ID_SUCURSAL"]),
                            NombreSucursal = dr["NOMBRE_SUCURSAL"].ToString()
                        },

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

                        oTipoIncentivo = new TipoIncentivo
                        {
                            IdTipoIncentivo = Convert.ToInt32(dr["ID_TIPO_INCENTIVO"]),
                            Nombre = dr["TIPO_INCENTIVO"].ToString()
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

        public bool Crear(Capa_Entidad.Incentivo obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection con = new SqlConnection(cn);
                SqlCommand cmd = new SqlCommand("SP_CREAR_INCENTIVO", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSUCURSAL", obj.IdSucursal);
                cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                cmd.Parameters.AddWithValue("@ID_TIPO_CANJE", obj.IdTipoCanje);
                cmd.Parameters.AddWithValue("@ID_TIPO_INCENTIVO", obj.IdTipoIncentivo);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@USUARIO_REGISTRO", obj.UsuarioRegistro);
                cmd.Parameters.AddWithValue("@DOCUMENTO_ADJUNTO", obj.DocumentoAdjunto ?? "");
                cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool Editar(Capa_Entidad.Incentivo obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection con = new SqlConnection(cn);
                SqlCommand cmd = new SqlCommand("SP_EDITAR_INCENTIVO", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_INCENTIVO", obj.IdIncentivo);
                cmd.Parameters.AddWithValue("@IDSUCURSAL", obj.IdSucursal);
                cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                cmd.Parameters.AddWithValue("@ID_TIPO_CANJE", obj.IdTipoCanje);
                cmd.Parameters.AddWithValue("@ID_TIPO_INCENTIVO", obj.IdTipoIncentivo);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@DOCUMENTO_ADJUNTO", obj.DocumentoAdjunto ?? "");
                cmd.Parameters.AddWithValue("@COMENTARIO", obj.Comentario ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool RegistrarPago(PagoIncentivo pago, out string mensaje, out string numeroDocumento)
        {
            mensaje = "";
            numeroDocumento = "";

            try
            {
                using SqlConnection conn = new SqlConnection(cn);

                SqlCommand cmd = new SqlCommand(
                    "SP_REGISTRAR_PAGO_INCENTIVO", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@ID_INCENTIVO", pago.IdIncentivo);

                cmd.Parameters.AddWithValue(
                    "@MONTO_TOTAL", pago.MontoTotal);

                cmd.Parameters.AddWithValue(
                    "@OBSERVACION", pago.Observacion ?? "");


                DataTable dt = new DataTable();

                dt.Columns.Add("ID_INCENTIVO", typeof(int));
                dt.Columns.Add("MONTO_PAGADO", typeof(decimal));
                dt.Columns.Add("ID_TIPO_DOC", typeof(int));
                dt.Columns.Add("NUMERO_CONFIRMACION", typeof(string));
                dt.Columns.Add("COMPROBANTE", typeof(string));
                dt.Columns.Add("USUARIO_PAGO", typeof(string));


                foreach (var d in pago.Detalles)
                {
                    dt.Rows.Add(
                        pago.IdIncentivo,
                        d.MontoPagado,
                        d.IdTipoDocumento,
                        d.NumeroConfirmacion,
                        d.RutaComprobante,
                        d.UsuarioPago
                    );
                }


                var param = cmd.Parameters.AddWithValue("@DETALLE", dt);

                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "T_DETALLE_PAGO_INCENTIVO";


                cmd.Parameters.Add(
                    "@ID_PAGO_INCENTIVO_OUT",
                    SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.Parameters.Add(
                    "@NUMERO_DOCUMENTO_OUT",
                    SqlDbType.VarChar, 20).Direction =
                    ParameterDirection.Output;


                conn.Open();

                cmd.ExecuteNonQuery();


                numeroDocumento =
                    cmd.Parameters["@NUMERO_DOCUMENTO_OUT"]
                    .Value.ToString();

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }
    }

}
