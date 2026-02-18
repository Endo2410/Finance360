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
    public class CD_AlertaUsuario
    {
        private readonly string cadenaConexion = Conexion.cn;

        // 🔹 LISTAR
        public List<AlertaUsuario> ObtenerAlertaUsuario()
        {
            List<AlertaUsuario> lista = new List<AlertaUsuario>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ALERTA_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new AlertaUsuario
                    {
                        IdAlertaUsuario = Convert.ToInt32(dr["ID_ALERTA_USUARIO"]),
                        IdTipoAlerta = Convert.ToInt32(dr["ID_TIPO_ALERTA"]),
                        IdUsuario = Convert.ToInt32(dr["ID_USUARIO"]),
                        IdEstado = Convert.ToInt32(dr["IDESTADO"]),

                        oTipoAlerta = new TipoAlerta
                        {
                            IdTipoAlerta = Convert.ToInt32(dr["ID_TIPO_ALERTA"]),
                            Codigo = dr["TipoAlerta"].ToString()
                        },

                        oUsuario = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["ID_USUARIO"]),
                            NombreUsuario = dr["NombreUsuario"].ToString()
                        },

                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["Estado"].ToString()
                        }
                    });
                }
            }

            return lista;
        }

        // 🔹 CREAR
        public bool CrearAlertaUsuario(AlertaUsuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_ALERTA_USUARIO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_TIPO_ALERTA", obj.IdTipoAlerta);
                    cmd.Parameters.AddWithValue("@ID_USUARIO", obj.IdUsuario);
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

        // 🔹 EDITAR
        public bool EditarAlertaUsuario(AlertaUsuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_ALERTA_USUARIO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_ALERTA_USUARIO", obj.IdAlertaUsuario);
                    cmd.Parameters.AddWithValue("@ID_TIPO_ALERTA", obj.IdTipoAlerta);
                    cmd.Parameters.AddWithValue("@ID_USUARIO", obj.IdUsuario);
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


        //tipos de alerta
        public List<TipoAlerta> ListarTipoAlerta()
        {
            List<TipoAlerta> lista = new List<TipoAlerta>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_TIPO_ALERTA_", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new TipoAlerta
                    {
                        IdTipoAlerta = Convert.ToInt32(dr["ID_TIPO_ALERTA"]),
                        Codigo = dr["CODIGO"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}
