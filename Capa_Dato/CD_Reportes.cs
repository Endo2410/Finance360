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
    public class CD_Reportes
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<ComprasCliente> ObtenerComprasCliente(string accountNumber, DateTime inicio, DateTime fin)
        {
            List<ComprasCliente> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_REPORTE_COMPRAS_CLIENTE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACCOUNTNUMBER", accountNumber);
                cmd.Parameters.AddWithValue("@FECHA_INICIO", inicio);
                cmd.Parameters.AddWithValue("@FECHA_FIN", fin);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ComprasCliente
                        {
                            Farmacia = dr["Farmacia"].ToString(),
                            StoreID = Convert.ToInt32(dr["StoreID"]),
                            Time = Convert.ToDateTime(dr["Time"]),
                            TransactionNumber = dr["TransactionNumber"].ToString(),
                            AccountNumber = dr["AccountNumber"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Total = Convert.ToDecimal(dr["Total"])
                        });
                    }
                }
            }

            return lista;
        }

        public List<ReporteCompras> ObtenerReporte(DateTime inicio, DateTime fin, string proveedor, string laboratorio)
        {
            List<ReporteCompras> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_REPORTE_COMPRAS_LAB_PROV", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FECHA_INICIO", inicio);
                cmd.Parameters.AddWithValue("@FECHA_FIN", fin);
                cmd.Parameters.AddWithValue("@PROVEEDOR", (object)proveedor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LABORATORIO", (object)laboratorio ?? DBNull.Value);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ReporteCompras
                        {
                            Proveedor = dr["Proveedor"].ToString(),
                            Laboratorio = dr["Laboratorio"].ToString(),
                            TotalComprado = Convert.ToDecimal(dr["TotalComprado"])
                        });
                    }
                }
            }

            return lista;
        }

        //ordenes sin recibir
        public List<OrdenSinRecibir> ObtenerOrdenessinrecibir(DateTime inicio, DateTime fin)
        {
            List<OrdenSinRecibir> lista = new();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_ORDENES_SIN_RECIBIR", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FECHA_INICIO", inicio);
                cmd.Parameters.AddWithValue("@FECHA_FIN", fin);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new OrdenSinRecibir
                        {
                            PONumber = dr["PONumber"].ToString(),
                            DateCreated = Convert.ToDateTime(dr["DateCreated"]),
                            Farmacia = dr["Farmacia"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
