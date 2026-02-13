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
    public class CD_ModalidadOperacion
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<ModalidadOperacion> ObtenerModalidades()
        {
            List<ModalidadOperacion> lista = new();

            using (SqlConnection conn = new(cadenaConexion))
            {
                SqlCommand cmd = new("SP_OBTENER_MODALIDAD_OPERACION", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new ModalidadOperacion
                    {
                        IdModalidadOp = Convert.ToInt32(dr["ID_MODALIDAD_OP"]),
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
            }
            return lista;
        }

        public bool Crear(ModalidadOperacion obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_CREAR_MODALIDAD_OPERACION", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

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

        public bool Editar(ModalidadOperacion obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_EDITAR_MODALIDAD_OPERACION", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_MODALIDAD_OP", obj.IdModalidadOp);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@IDESTADO", obj.IdEstado);

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
