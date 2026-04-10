using Capa_Entidad;
using Capa_Entidad.CE_Rebate;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.Rebate
{
    public class CD_CriterioRebate
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<CriterioRebate> ObtenerCriterios()
        {
            List<CriterioRebate> lista = new();

            using SqlConnection conn = new(cadenaConexion);
            SqlCommand cmd = new("SP_OBTENER_CRITERIO_REBATE", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new CriterioRebate
                {
                    IdCriterio = Convert.ToInt32(dr["ID_CRITERIO"]),
                    Nombre = dr["NOMBRE"].ToString(),
                    Operador = dr["OPERADOR"].ToString(),
                    Descripcion = dr["DESCRIPCION"].ToString(),
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

        public bool Crear(CriterioRebate obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_CREAR_CRITERIO_REBATE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@OPERADOR", obj.Operador);
                cmd.Parameters.AddWithValue("@DESCRIPCION", obj.Descripcion);
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

        public bool Editar(CriterioRebate obj, out string mensaje)
        {
            mensaje = "";
            try
            {
                using SqlConnection conn = new(cadenaConexion);
                SqlCommand cmd = new("SP_EDITAR_CRITERIO_REBATE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_CRITERIO", obj.IdCriterio);
                cmd.Parameters.AddWithValue("@NOMBRE", obj.Nombre);
                cmd.Parameters.AddWithValue("@OPERADOR", obj.Operador);
                cmd.Parameters.AddWithValue("@DESCRIPCION", obj.Descripcion);
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
