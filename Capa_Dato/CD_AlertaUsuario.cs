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

        // LISTAR
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
                            NombreUsuario = dr["NOMBRES"].ToString() + " " + dr["APELLIDOS"].ToString()
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

        public bool GuardarAlertas(AlertaUsuario obj, out string mensaje)
        {
            mensaje = "";
            bool resultado = true;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_ALERTAS_POR_USUARIO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@ListaAlertas", string.Join(",", obj.TiposAlerta));
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstado);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
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

        // OBTENER ALERTAS POR USUARIO
        public List<Alerta> ObtenerAlertasUsuario(int idUsuario)
        {
            List<Alerta> lista = new List<Alerta>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_ALERTAS_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUSUARIO", idUsuario);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Alerta
                    {
                        IdAlerta = Convert.ToInt32(dr["ID_ALERTA"]),
                        Mensaje = dr["MENSAJE"].ToString(),
                        FechaAlerta = Convert.ToDateTime(dr["FECHA_ALERTA"]),
                        Vista = Convert.ToBoolean(dr["VISTA"])
                    });
                }
            }

            return lista;
        }

        //CONTADOR
        public int ContarAlertas(int idUsuario)
        {
            int total = 0;

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_CONTAR_ALERTAS_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUSUARIO", idUsuario);

                conn.Open();
                total = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return total;
        }

        //MARCAR TODAS COMO VISTAS
        public void MarcarTodasComoVistas(int idUsuario)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_MARCAR_TODAS_ALERTAS_VISTA", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUSUARIO", idUsuario);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
