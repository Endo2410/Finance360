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
    public class CD_NotaCredito
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<NotaCredito> ListarNotasCredito()
        {
            var lista = new List<NotaCredito>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_NOTAS_CREDITO", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new NotaCredito
                        {
                            IdNC = Convert.ToInt32(dr["ID_NC"]),
                            NumeroNC = dr["NUMERO_NC"].ToString(),
                            FechaEmision = Convert.ToDateTime(dr["FECHA_EMISION"]),
                            Monto = Convert.ToDecimal(dr["MONTO"]),
                            TipoOrigen = dr["TIPO_ORIGEN"].ToString(),
                            IdOrigen = Convert.ToInt32(dr["ID_ORIGEN"]),
                            FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                            NumeroDocumentoOrigen = dr["NUMERO_DOCUMENTO_ORIGEN"].ToString(),
                            NumeroDocumentoConfirmacion = dr["NUMERO_DOCUMENTO_CONFIRMACION"].ToString(),  
                            DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"].ToString(),             

                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            oProveedor = new Proveedor
                            {
                                IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                                NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                            },
                            oEstado = new Estado
                            {
                                IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                                Nombre = dr["ESTADO"].ToString()
                            }
                        });
                    }
                }
            }
            return lista;
        }

        // Obtener detalle de pagos de una nota
        public List<DetallePagoNC> ObtenerDetallePago(int idNC)
        {
            var lista = new List<DetallePagoNC>();

            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_DETALLE_PAGO_NC", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_NC", idNC);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DetallePagoNC
                        {
                            NumeroConfirmacion = dr["NUMERO_CONFIRMACION"].ToString(),
                            Comprobante = dr["COMPROBANTE"].ToString(),
                            UsuarioAplica = dr["USUARIO_APLICA"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"])
                        });
                    }
                }
            }

            return lista;
        }
    }
}
