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


        //DEPARTAMENTO
        public List<Departamento> ObtenerDepartamentos()
        {
            List<Departamento> lista = new List<Departamento>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_DEPARTAMENTOS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Departamento
                    {
                        IdDepartamento = Convert.ToInt32(dr["ID_DEPARTAMENTO"]),
                        IdOrigen = Convert.ToInt32(dr["ID_ORIGEN"]),
                        FechaActualizacion = Convert.ToDateTime(dr["FECHA_ACTUALIZACION"]),
                        NombreDepartamento = dr["NOMBRE_DEPARTAMENTO"].ToString()
                    });
                }
            }

            return lista;
        }

        public object SincronizarDepartamentos()
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_SINCRONIZAR_DEPARTAMENTOS", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                int actualizados = 0;
                int insertados = 0;

                if (dr.Read())
                {
                    actualizados = Convert.ToInt32(dr["DepartamentosActualizados"]);
                    insertados = Convert.ToInt32(dr["DepartamentosInsertados"]);
                }

                return new
                {
                    actualizados,
                    insertados
                };
            }
        }


        public List<ItemDepartamento> LISTAR_ITEM(int? ID_DEPARTAMENTO)
        {
            List<ItemDepartamento> lista = new List<ItemDepartamento>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_ITEM_POR_DEPARTAMENTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_DEPARTAMENTO", (object)ID_DEPARTAMENTO ?? DBNull.Value);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new ItemDepartamento
                    {
                        ID_DEPARTAMENTO = Convert.ToInt32(dr["ID_DEPARTAMENTO"]),
                        NOMBRE_DEPARTAMENTO = dr["NOMBRE_DEPARTAMENTO"].ToString(),

                        ID_ITEM = Convert.ToInt32(dr["ID_ITEM"]),
                        DESCRIPTION = dr["Description"].ToString(),
                        ITEMLOOKUPCODE = dr["ItemLookupCode"].ToString()
                    });
                }
            }

            return lista;
        }

    }
}
