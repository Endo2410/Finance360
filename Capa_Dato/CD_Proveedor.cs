using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace Capa_Dato
{
    public class CD_Proveedor
    {
        private readonly string cadenaConexion = Conexion.cn;

        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> lista = new List<Proveedor>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_PROVEEDORES", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Proveedor
                    {
                        IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                        Pais = dr["PAIS"].ToString(),
                        HQID = Convert.ToInt32(dr["HQID"]),
                        FechaActualizacion = Convert.ToDateTime(dr["FECHA_ACTUALIZACION"]),
                        NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString(),
                        NombreContacto = dr["NOMBRE_CONTACTO"].ToString(),
                        Ciudad = dr["CIUDAD"].ToString(),
                        Direccion = dr["DIRECCION"].ToString(),
                        Correo = dr["CORREO"].ToString(),
                        SitioWeb = dr["SITIO_WEB"].ToString(),
                        Ruc = dr["Ruc"].ToString(),
                        NumeroTelefono = dr["NUMERO_TELEFONO"].ToString(),
                        Fax = dr["FAX"].ToString(),
                        Terminos = dr["TERMINOS"].ToString()
                    });
                }
            }

            return lista;
        }

        public object SincronizarProveedores()
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_SINCRONIZAR_PROVEEDORES", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                int actualizados = 0;
                int insertados = 0;

                if (dr.Read())
                {
                    actualizados = Convert.ToInt32(dr["ProveedoresActualizados"]);
                    insertados = Convert.ToInt32(dr["ProveedoresInsertados"]);
                }

                return new
                {
                    actualizados,
                    insertados
                };
            }
        }

    }
}
