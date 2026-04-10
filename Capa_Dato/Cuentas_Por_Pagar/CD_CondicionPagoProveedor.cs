using Capa_Entidad;
using Capa_Entidad.Cuentas_Por_Pagar;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Capa_Dato.Cuentas_Pagar
{
    public class CD_CondicionPagoProveedor
    {
        private readonly string cn = Conexion.cn;

        public List<CondicionPagoProveedor> Obtener()
        {
            List<CondicionPagoProveedor> lista = new();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_CONDICION_PAGO_PROVEEDOR", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new CondicionPagoProveedor
                    {
                        IdCondicion = Convert.ToInt32(dr["IDCONDICION"]),
                        DiasCredito = Convert.ToInt32(dr["DIAS_CREDITO"]),

                        oProveedor = new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(dr["ID_PROVEEDOR"]),
                            NombreProveedor = dr["NOMBRE_PROVEEDOR"].ToString()
                        },

                        oTipoRetencion = dr["ID_TIPO_RETENCION"] == DBNull.Value ? null :
                        new TipoRetencion
                        {
                            IdTipoRetencion = Convert.ToInt32(dr["ID_TIPO_RETENCION"]),
                            Nombre = dr["RETENCION"].ToString()
                        },

                        oTipoDescuento = dr["ID_TIPO_DESCUENTO"] == DBNull.Value ? null :
                        new TipoDescuentoPP
                        {
                            IdTipoDescuento = Convert.ToInt32(dr["ID_TIPO_DESCUENTO"]),
                            Nombre = dr["DESCUENTO"].ToString()
                        }
                    });
                }
            }

            return lista;
        }

        public bool Crear(CondicionPagoProveedor obj, out string mensaje)
        {
            mensaje = "";

            try
            {
                using (SqlConnection con = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREAR_CONDICION_PAGO_PROVEEDOR", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@DIAS_CREDITO", obj.DiasCredito);
                    cmd.Parameters.AddWithValue("@ID_TIPO_RETENCION", (object)obj.IdTipoRetencion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID_TIPO_DESCUENTO", (object)obj.IdTipoDescuento ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool Editar(CondicionPagoProveedor obj, out string mensaje)
        {
            mensaje = "";

            try
            {
                using (SqlConnection con = new SqlConnection(cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_CONDICION_PAGO_PROVEEDOR", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDCONDICION", obj.IdCondicion);
                    cmd.Parameters.AddWithValue("@ID_PROVEEDOR", obj.IdProveedor);
                    cmd.Parameters.AddWithValue("@DIAS_CREDITO", obj.DiasCredito);
                    cmd.Parameters.AddWithValue("@ID_TIPO_RETENCION", (object)obj.IdTipoRetencion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID_TIPO_DESCUENTO", (object)obj.IdTipoDescuento ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }
    }
}