using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.Vencido
{
    public class CD_PagoVencido
    {
        private readonly string cn = Conexion.cn;

        public bool RegistrarPago(PagoVencido pago, out string mensaje, out string numeroDocumento)
        {
            mensaje = "";
            numeroDocumento = "";
            bool resultado = false;

            try
            {
                using SqlConnection conn = new SqlConnection(cn);
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR_PAGO_VENCIDO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_VENCIDO", pago.IdVencido);
                cmd.Parameters.AddWithValue("@FECHA_DOCUMENTO", pago.FechaDocumento);
                cmd.Parameters.AddWithValue("@MONTO_TOTAL", pago.MontoTotal);
                cmd.Parameters.AddWithValue("@OBSERVACION", pago.Observacion ?? "");

                // Detalle pago
                DataTable dt = new DataTable();
                dt.Columns.Add("MONTO_PAGADO", typeof(decimal));
                dt.Columns.Add("ID_TIPO_DOCUMENTO", typeof(int));
                dt.Columns.Add("NUMERO_CONFIRMACION", typeof(string));
                dt.Columns.Add("COMPROBANTE", typeof(string));
                dt.Columns.Add("USUARIO_PAGO", typeof(string));

                foreach (var d in pago.DetalleCuotas)
                    dt.Rows.Add(d.MontoPagado, d.IdTipoDocumento, d.NumeroConfirmacion, d.RutaComprobante, d.usuarioPago);

                SqlParameter param = cmd.Parameters.AddWithValue("@DETALLE", dt);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "T_DETALLE_PAGO_VENCIDO";

                SqlParameter outId = new SqlParameter("@ID_PAGO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);

                SqlParameter outNumeroDoc = new SqlParameter("@NUMERO_DOCUMENTO_OUT", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outNumeroDoc);

                DataTable dtRet = new();
                dtRet.Columns.Add("ID_TIPO_RETENCION", typeof(int));
                dtRet.Columns.Add("PORCENTAJE", typeof(decimal));
                dtRet.Columns.Add("MONTO_RETENIDO", typeof(decimal));

                foreach (var r in pago.Retenciones)
                    dtRet.Rows.Add(r.IdTipoRetencion, r.Porcentaje, r.MontoRetenido);

                var paramRet = cmd.Parameters.AddWithValue("@RETENCIONES", dtRet);
                paramRet.SqlDbType = SqlDbType.Structured;
                paramRet.TypeName = "T_RETENCIONES_VENCIDO";


                conn.Open();
                cmd.ExecuteNonQuery();

                resultado = true;
                numeroDocumento = outNumeroDoc.Value.ToString();
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
