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
    public class CD_Rol
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Rol> Obtener()
        {
            List<Rol> lista = new();

            using SqlConnection conn = new(cadenaConexion);
            SqlCommand cmd = new("SP_OBTENER_ROL", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Rol
                {
                    IdRol = (int)dr["IdRol"],
                    Nombre = dr["Nombre"].ToString(),
                    Descripcion = dr["Descripcion"].ToString(),
                    IdEstado = (int)dr["IdEstado"],
                    oEstado = new Estado
                    {
                        IdEstado = (int)dr["IdEstado"],
                        Nombre = dr["Estado"].ToString()
                    }
                });
            }
            return lista;
        }

        public bool Crear(Rol obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_CREAR_ROL", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                cmd.Parameters.AddWithValue("@IdEstado", obj.oEstado.IdEstado);

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

        public bool Editar(Rol obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_EDITAR_ROL", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdRol", obj.IdRol);
                cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                cmd.Parameters.AddWithValue("@IdEstado", obj.oEstado.IdEstado);

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
