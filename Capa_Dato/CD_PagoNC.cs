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

     public class CD_PagoNC
     {
            private readonly string cn = Conexion.cn;

            public bool AplicarNotasCredito(List<NotaCredito> notas, out List<string> mensajes, out List<string> numerosDocumentos)
            {
                mensajes = new List<string>();
                numerosDocumentos = new List<string>();
                bool resultado = true;

                foreach (var nota in notas)
                {
                    try
                    {
                        using SqlConnection conn = new SqlConnection(cn);
                        SqlCommand cmd = new SqlCommand("SP_APLICAR_NOTA_CREDITO", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@ID_NC", nota.IdNC);
                        cmd.Parameters.AddWithValue("@OBSERVACION", nota.Observacion ?? "");

                        // Crear tabla tipo para cheques
                        DataTable dt = new DataTable();
                        dt.Columns.Add("NUMERO_CONFIRMACION", typeof(string));
                        dt.Columns.Add("COMPROBANTE", typeof(string));
                        dt.Columns.Add("USUARIO_PAGO", typeof(string));

                        if (nota.DetallePagos != null)
                        {
                            foreach (var d in nota.DetallePagos)
                            {
                                dt.Rows.Add(d.NumeroConfirmacion, d.RutaComprobante, d.UsuarioPago);
                            }
                        }

                        SqlParameter param = cmd.Parameters.AddWithValue("@DETALLE", dt);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "T_DETALLE_PAGO_NC";

                        SqlParameter outId = new SqlParameter("@ID_PAGO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(outId);

                        SqlParameter outNumeroDoc = new SqlParameter("@NUMERO_DOCUMENTO_OUT", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(outNumeroDoc);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        numerosDocumentos.Add(outNumeroDoc.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        mensajes.Add($"Error al aplicar nota {nota.IdNC}: {ex.Message}");
                        resultado = false;
                    }
                }

                return resultado;
            }
    }
    
}
