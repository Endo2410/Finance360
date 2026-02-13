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
    public class CD_Moneda
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Moneda> ObtenerMonedas()
        {
            List<Moneda> lista = new List<Moneda>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_MONEDA", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Moneda m = new Moneda
                    {
                        IdMoneda = Convert.ToInt32(dr["ID_MONEDA"]),
                        Nombre = dr["NOMBRE"].ToString(),
                        Simbolo = dr["SIMBOLO"].ToString(),
                        IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                        FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["ESTADO"].ToString()
                        }
                    };
                    lista.Add(m);
                }
            }

            return lista;
        }

        public bool CrearMoneda(Moneda obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_MONEDA", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@SIMBOLO", obj.Simbolo);
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool EditarMoneda(Moneda obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_MONEDA", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_MONEDA", obj.IdMoneda);
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@SIMBOLO", obj.Simbolo);
                    cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
