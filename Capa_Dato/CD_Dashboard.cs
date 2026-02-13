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
    public class CD_Dashboard
    {
        private readonly string cn = Conexion.cn;

        public List<Dashboard> ObtenerTopProveedores(DateTime? fechaInicio, DateTime? fechaFin, int? idProveedor)
        {
            List<Dashboard> lista = new();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_TOP_PROVEEDORES_RESUMEN", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FECHA_INICIO", (object?)fechaInicio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FECHA_FIN", (object?)fechaFin ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ID_PROVEEDOR", (object?)idProveedor ?? DBNull.Value);

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Dashboard
                        {
                            Proveedor = dr["Proveedor"].ToString(),
                            Publicidad = Convert.ToDecimal(dr["Publicidad"]),
                            Rebate = Convert.ToDecimal(dr["Rebate"]),
                            Canjes = Convert.ToDecimal(dr["Canjes"]),
                            Vencido = Convert.ToDecimal(dr["Vencido"]),
                            TotalIngreso = Convert.ToDecimal(dr["TotalIngreso"])
                        });
                    }
                }
            }

            return lista;
        }


        public List<Dashboard> ObtenerTopProveedoresSaldoPendiente(int? idProveedor)
        {
            List<Dashboard> lista = new();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand(
                    "SP_TOP_PROVEEDORES_SALDO_PENDIENTE",
                    conn
                );
                cmd.CommandType = CommandType.StoredProcedure;

                // 🔹 Enviar parámetro (solo proveedor, NO fechas)
                cmd.Parameters.AddWithValue("@ID_PROVEEDOR",
                    (object?)idProveedor ?? DBNull.Value);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Dashboard
                        {
                            Proveedor = dr["Proveedor"].ToString(),

                            Publicidad = dr["Publicidad"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Publicidad"]),
                            Rebate = dr["Rebate"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Rebate"]),
                            Canjes = dr["Canjes"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Canjes"]),
                            Vencido = dr["Vencido"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Vencido"]),

                            SaldoPendiente = dr["SaldoPendiente"] == DBNull.Value
                                ? 0
                                : Convert.ToDecimal(dr["SaldoPendiente"])
                        });
                    }
                }
            }

            return lista;
        }


        public Dashboard ObtenerResumenFinanciero(DateTime? fechaInicio, DateTime? fechaFin, int? idProveedor)
        {
            using SqlConnection cn = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("SP_DASHBOARD_FINANCIERO_RESUMEN", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FECHA_INICIO", (object?)fechaInicio ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FECHA_FIN", (object?)fechaFin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ID_PROVEEDOR", (object?)idProveedor ?? DBNull.Value);

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Dashboard
                {
                    SaldoPendiente = Convert.ToDecimal(dr["SaldoPendiente"]),
                    Cobrado = Convert.ToDecimal(dr["Cobrado"]),
                    PorcentajeRecuperacion = Convert.ToDecimal(dr["PorcentajeRecuperacion"])
                };
            }

            return new Dashboard();
        }



        public Dashboard ObtenerCantidadActivos()
        {
            using SqlConnection cn = new SqlConnection(Conexion.cn);
            SqlCommand cmd = new SqlCommand("SP_DASHBOARD_ACTIVOS_RESUMEN", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Dashboard
                {
                    PublicidadActiva = Convert.ToInt32(dr["PublicidadActiva"]),
                    RebateActivo = Convert.ToInt32(dr["RebateActivo"]),
                    VencidoActivo = Convert.ToInt32(dr["VencidoActivo"]),
                    CanjeActivo = Convert.ToInt32(dr["CanjeActivo"])
                };
            }

            return new Dashboard();
        }

    }
}
