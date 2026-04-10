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
    public class CD_OrdenCompra
    {
        private readonly string cadenaConexion = Conexion.cn;

   
        //nuevos 

        public List<OrdenCompra> ObtenerOrdenes(DateTime inicio, DateTime fin)
        {
            List<OrdenCompra> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ORDENES_COMPRA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FECHA_INICIO", inicio);
                cmd.Parameters.AddWithValue("@FECHA_FIN", fin);

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new OrdenCompra
                        {
                            IdOrden = Convert.ToInt32(dr["IDORDEN"]),
                            IdOrdenCompra = Convert.ToInt32(dr["IDORDENCOMPRA"]),
                            HQID = Convert.ToInt32(dr["HQID"]),
                            Proveedor = dr["PROVEEDOR"].ToString(),
                            NumeroOrden = dr["NUMERO_ORDEN"].ToString(),
                            StatusOrden = dr["STATUS_ORDEN"].ToString(),
                            Factura = dr["FACTURA"].ToString(),
                            FechaCreacion = Convert.ToDateTime(dr["FECHA_CREACION"]),
                            Total = Convert.ToDecimal(dr["TOTAL"]),
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            NombreEstado = dr["NOMBRE_ESTADO"].ToString(),
                            Comentario = dr["COMENTARIO"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public int InsertarOrdenesNuevas()
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_INSERTAR_ORDENES_COMPRA", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
            }
        }
    }
}
