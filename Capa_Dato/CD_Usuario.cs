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
    public class CD_Usuario
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        Clave = dr["Clave"].ToString(),
                        Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        IdEstado = Convert.ToInt32(dr["IdEstado"]),
                        oRol = new Rol
                        {
                            IdRol = Convert.ToInt32(dr["IdRol"]),
                            Nombre = dr["Rol"].ToString()
                        },
                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IdEstado"]),
                            Nombre = dr["Estado"].ToString()
                        }
                    };
                    lista.Add(usuario);
                }
            }

            return lista;
        }



        public bool CrearUsuario(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_USUARIO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@NombreUsuario", obj.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstado);

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

        public bool EditarUsuario(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_USUARIO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@NombreUsuario", obj.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                    cmd.Parameters.AddWithValue("@IdEstado", obj.IdEstado);


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


        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CLAVE =@NUEVA_CLAVE, REESTABLECER = 0 WHERE IDUSUARIO = @ID ", oconexion);
                    cmd.Parameters.AddWithValue("@ID", idusuario);
                    cmd.Parameters.AddWithValue("@NUEVA_CLAVE", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool RestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CLAVE = @CLAVE, REESTABLECER = 1 WHERE IDUSUARIO = @ID ", oconexion);
                    cmd.Parameters.AddWithValue("@ID", idusuario);
                    cmd.Parameters.AddWithValue("@CLAVE", clave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
        
    }
}
