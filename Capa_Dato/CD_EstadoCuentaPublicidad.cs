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
    public class CD_EstadoCuentaPublicidad
    {

        private readonly string cn = Conexion.cn;

        public List<EstadoCuentaPublicidad> ObtenerPendientes()
        {
            List<EstadoCuentaPublicidad> lista = new();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ESTADO_CUENTA_PUBLICIDAD", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new EstadoCuentaPublicidad
                        {
                            IdEstadoCuenta = Convert.ToInt32(dr["ID_ESTADO_CUENTA"]),
                            IdCampania = Convert.ToInt32(dr["ID_CAMPANIA"]),
                            NumeroCampania = dr["NUMERO_CAMPANIA"].ToString(),
                            NombreCampania = dr["NOMBRE_CAMPANIA"].ToString(),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString(),
                            NumeroCuota = Convert.ToInt32(dr["NUMERO_CUOTA"]),
                            FechaPagoProgramada = Convert.ToDateTime(dr["FECHA_PAGO_PROGRAMADA"]),
                            MontoCuota = Convert.ToDecimal(dr["MONTO_CUOTA"]),
                            Estado = dr["ESTADO"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}
