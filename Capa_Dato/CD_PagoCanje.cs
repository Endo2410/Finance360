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
    public class CD_PagoCanje
    {
        private readonly string cn = Conexion.cn;

        public bool RegistrarPago(PagoCanje pago, out string mensaje, out string numeroDocumento)
        {
            mensaje = "";
            numeroDocumento = "";

            try
            {
                using SqlConnection conn = new(cn);
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR_PAGO_CANJE", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ID_CANJE", pago.IdCanje);
                cmd.Parameters.AddWithValue("@MONTO_TOTAL", pago.MontoTotal);
                cmd.Parameters.AddWithValue("@OBSERVACION", pago.Observacion ?? "");

                DataTable dt = new();
                dt.Columns.Add("ID_CANJE", typeof(int));
                dt.Columns.Add("MONTO_PAGADO", typeof(decimal));
                dt.Columns.Add("ID_TIPO_DOC", typeof(int));
                dt.Columns.Add("NUMERO_CONFIRMACION", typeof(string));
                dt.Columns.Add("COMPROBANTE", typeof(string));
                dt.Columns.Add("USUARIO_PAGO", typeof(string));

                foreach (var d in pago.DetalleCanjes)
                    dt.Rows.Add(d.IdCanjeDetalle, d.MontoPagado, d.IdTipoDocumento, d.NumeroConfirmacion, d.RutaComprobante, d.UsuarioPago);

                var param = cmd.Parameters.AddWithValue("@DETALLE", dt);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "T_DETALLE_PAGO_CANJE";

                cmd.Parameters.Add("@ID_PAGO_CANJE_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@NUMERO_DOCUMENTO_OUT", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;

                DataTable dtRet = new();
                dtRet.Columns.Add("ID_TIPO_RETENCION", typeof(int));
                dtRet.Columns.Add("PORCENTAJE", typeof(decimal));
                dtRet.Columns.Add("MONTO_RETENIDO", typeof(decimal));

                foreach (var r in pago.Retenciones)
                    dtRet.Rows.Add(r.IdTipoRetencion, r.Porcentaje, r.MontoRetenido);

                var paramRet = cmd.Parameters.AddWithValue("@RETENCIONES", dtRet);
                paramRet.SqlDbType = SqlDbType.Structured;
                paramRet.TypeName = "T_RETENCIONES_CANJE";

                conn.Open();
                cmd.ExecuteNonQuery();

                numeroDocumento = cmd.Parameters["@NUMERO_DOCUMENTO_OUT"].Value.ToString();
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

