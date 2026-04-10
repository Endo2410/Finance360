using Capa_Entidad.Cuentas_Por_Pagar;
using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.Cuentas_Por_Pagar
{
    public class CD_TipoDescuentoPP
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<TipoDescuentoPP> Obtener()
        {
            List<TipoDescuentoPP> lista = new();

            using SqlConnection conn = new(cadenaConexion);
            SqlCommand cmd = new("SP_OBTENER_TIPO_DESCUENTO_PP", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new TipoDescuentoPP
                {
                    IdTipoDescuento = Convert.ToInt32(dr["ID_TIPO_DESCUENTO"]),
                    Nombre = dr["NOMBRE"].ToString(),
                    Porcentaje = Convert.ToDecimal(dr["PORCENTAJE"]),
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

        public bool Crear(TipoDescuentoPP obj, out string mensaje)
        {
            mensaje = "";

            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_CREAR_TIPO_DESCUENTO_PP", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@PORCENTAJE", obj.Porcentaje);
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

        public bool Editar(TipoDescuentoPP obj, out string mensaje)
        {
            mensaje = "";

            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_EDITAR_TIPO_DESCUENTO_PP", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_TIPO_DESCUENTO", obj.IdTipoDescuento);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@PORCENTAJE", obj.Porcentaje);
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
