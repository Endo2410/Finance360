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
    public class CD_Modalidad
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Modalidad> ObtenerModalidades()
        {
            List<Modalidad> lista = new List<Modalidad>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_MODALIDAD", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Modalidad m = new Modalidad
                    {
                        IdModalidad = Convert.ToInt32(dr["ID_MODALIDAD"]),
                        Nombre = dr["NOMBRE"].ToString(),
                        TipoIntervalo = dr["TIPO_INTERVALO"].ToString(),
                        ValorIntervalo = Convert.ToInt32(dr["VALOR_INTERVALO"]),
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

        public bool CrearModalidad(Modalidad obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_MODALIDAD", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@TIPO_INTERVALO", obj.TipoIntervalo);
                    cmd.Parameters.AddWithValue("@VALOR_INTERVALO", obj.ValorIntervalo);
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

        public bool EditarModalidad(Modalidad obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_MODALIDAD", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_MODALIDAD", obj.IdModalidad);
                    cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                    cmd.Parameters.AddWithValue("@TIPO_INTERVALO", obj.TipoIntervalo);
                    cmd.Parameters.AddWithValue("@VALOR_INTERVALO", obj.ValorIntervalo);
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
