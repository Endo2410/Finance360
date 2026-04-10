using Capa_Entidad.CE_Incentivo;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Capa_Dato.Incentivo
{
    public class CD_PorcentajeComisiones
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<PorcentajeComisiones> Obtener()
        {
            List<PorcentajeComisiones> lista = new();
            using SqlConnection conn = new(cadenaConexion);
            SqlCommand cmd = new("SP_OBTENER_PORCENTAJE_COMISION", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new PorcentajeComisiones
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    Cargo = dr["CARGO"].ToString(),
                    MontoMin = Convert.ToDecimal(dr["MONTO_MINIMO"]),
                    MontoMax = Convert.ToDecimal(dr["MONTO_MAX"]),
                    Porcentaje = Convert.ToDecimal(dr["PORCENTAJE"])
                });
            }
            return lista;
        }

        public bool Crear(PorcentajeComisiones obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_CREAR_PORCENTAJE_COMISION", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CARGO", obj.Cargo);
                cmd.Parameters.AddWithValue("@MONTO_MINIMO", obj.MontoMin);
                cmd.Parameters.AddWithValue("@MONTO_MAX", obj.MontoMax);
                cmd.Parameters.AddWithValue("@PORCENTAJE", obj.Porcentaje);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool Editar(PorcentajeComisiones obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_EDITAR_PORCENTAJE_COMISION", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", obj.Id);
                cmd.Parameters.AddWithValue("@CARGO", obj.Cargo);
                cmd.Parameters.AddWithValue("@MONTO_MINIMO", obj.MontoMin);
                cmd.Parameters.AddWithValue("@MONTO_MAX", obj.MontoMax);
                cmd.Parameters.AddWithValue("@PORCENTAJE", obj.Porcentaje);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }
    }
}
