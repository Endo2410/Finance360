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
    public class CD_TipoCanje
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<TipoCanje> Obtener()
        {
            List<TipoCanje> lista = new();

            using SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd = new SqlCommand("SP_OBTENER_TIPO_CANJE", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new TipoCanje
                {
                    IdTipoCanje = Convert.ToInt32(dr["ID_TIPO_CANJE"]),
                    Nombre = dr["NOMBRE"].ToString(),
                    IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                    FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"]),
                    oEstado = new Estado
                    {
                        IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                        Nombre = dr["ESTADO"].ToString()
                    }
                });
            }

            return lista;
        }

        public bool Crear(TipoCanje obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new SqlConnection(cadenaConexion);
                SqlCommand cmd = new SqlCommand("SP_CREAR_TIPO_CANJE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@IDESTADO", obj.oEstado.IdEstado);

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

        public bool Editar(TipoCanje obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new SqlConnection(cadenaConexion);
                SqlCommand cmd = new SqlCommand("SP_EDITAR_TIPO_CANJE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_TIPO_CANJE", obj.IdTipoCanje);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@IDESTADO", obj.oEstado.IdEstado);

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
