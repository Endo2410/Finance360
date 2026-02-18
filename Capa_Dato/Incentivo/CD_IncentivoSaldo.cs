using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato.Incentivo
{
    public class CD_IncentivoSaldo
    {
        private readonly string cn = Conexion.cn;

        // Obtener saldo de la sucursal
        public IncentivoSaldo ObtenerSaldo(int idSucursal)
        {
            IncentivoSaldo obj = new IncentivoSaldo();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand(
                    "SP_OBTENER_SALDO_INCENTIVO_SUCURSAL",
                    con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@IDSUCURSAL",
                    idSucursal);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj.TotalEntrada =
                        Convert.ToDecimal(dr["TOTAL_ENTRADA"]);

                    obj.TotalSalida =
                        Convert.ToDecimal(dr["TOTAL_SALIDA"]);

                    obj.SaldoDisponible =
                        Convert.ToDecimal(dr["SALDO_DISPONIBLE"]);
                }
            }

            return obj;
        }

        // Obtener incentivos recibidos de la sucursal
        public List<IncentivoRecibido> ObtenerIncentivosRecibidos(int idSucursal)
        {
            List<IncentivoRecibido> lista = new List<IncentivoRecibido>();

            using (SqlConnection con = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_INCENTIVOS_RECIBIDOS_SUCURSAL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSUCURSAL", idSucursal);

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new IncentivoRecibido()
                        {
                            IdPagoIncentivo = Convert.ToInt32(dr["ID_PAGO_INCENTIVO"]),
                            Fecha = Convert.ToDateTime(dr["FECHA"]),
                            DocumentoPago = dr["DOCUMENTO_PAGO"].ToString(),
                            DocumentoIncentivo = dr["DOCUMENTO_INCENTIVO"].ToString(),
                            Incentivo = dr["INCENTIVO"].ToString(),
                            Proveedor = dr["PROVEEDOR"].ToString(),
                            Monto = Convert.ToDecimal(dr["MONTO"])
                        });
                    }
                }
            }

            return lista;
        }

        //Registrar movinmiento de saldo 
        public bool Registrar(IncentivoMovimiento obj)
        {
            bool respuesta = false;

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("sp_registrar_uso_incentivo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSUCURSAL", obj.IdSucursal);
                cmd.Parameters.AddWithValue("@ID_TIPO_USO", obj.IdTipoUso);
                cmd.Parameters.AddWithValue("@MONTO", obj.Monto);
                cmd.Parameters.AddWithValue("@USUARIO", obj.UsuarioRegistro);
                cmd.Parameters.AddWithValue("@OBSERVACION", obj.Observacion);
                cmd.Parameters.AddWithValue("@COMPROBANTE", obj.Comprobante);

                con.Open();

                respuesta = cmd.ExecuteNonQuery() > 0;
            }

            return respuesta;
        }

        // Obtener usos del incentivo por sucursal
        public List<IncentivoMovimiento> ObtenerUsos(int idSucursal)
        {
            List<IncentivoMovimiento> lista = new List<IncentivoMovimiento>();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand(
                    "SP_OBTENER_USOS_INCENTIVO_SUCURSAL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDSUCURSAL", idSucursal);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new IncentivoMovimiento()
                        {
                            FechaMovimiento = Convert.ToDateTime(dr["FECHA"]),
                            TipoUsoNombre = dr["TIPO_USO"].ToString(),
                            Monto = Convert.ToDecimal(dr["MONTO"]),
                            Comprobante = dr["COMPROBANTE"].ToString(),
                            UsuarioRegistro = dr["USUARIO"].ToString(),
                            Observacion = dr["OBSERVACION"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
