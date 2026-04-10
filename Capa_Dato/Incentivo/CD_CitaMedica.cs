using Capa_Entidad.CE_Incentivo;
using Capa_Entidad;
using Capa_Entidad.Contabilidad_Alejandra;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Capa_Dato.Incentivo
{
    public class CD_CitaMedica
    {
        private readonly string cn = Conexion.cn;

        public List<CitaMedica> Obtener()
        {
            var lista = new List<CitaMedica>();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_CITAS_MEDICAS", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new CitaMedica
                    {
                        IdCita = Convert.ToInt32(dr["ID_CITA"]),
                        NombreCita = dr["NOMBRE_CITA"].ToString(),
                        DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"].ToString(),

                        oDepartamento = new Departamento
                        {
                            IdDepartamento = Convert.ToInt32(dr["ID_DEPARTAMENTO"]),
                            NombreDepartamento = dr["NOMBRE_DEPARTAMENTO"].ToString()
                        },

                        oSucursal = new E_Sucursales
                        {
                            IdSucursal = Convert.ToInt32(dr["ID_SUCURSAL"]),
                            NombreSucursal = dr["SUCURSAL"].ToString()
                        },

                        oEstado = new Estado
                        {
                            IdEstado = Convert.ToInt32(dr["IDESTADO"]),
                            Nombre = dr["ESTADO"].ToString()
                        },

                        Fechas = new List<DateTime>()
                    });
                }

                dr.Close();

                // 🔥 AQUÍ CARGAS LAS FECHAS
                foreach (var cita in lista)
                {
                    SqlCommand cmdFecha = new SqlCommand("SP_OBTENER_FECHAS_CITA", con);
                    cmdFecha.CommandType = CommandType.StoredProcedure;

                    cmdFecha.Parameters.AddWithValue("@ID_CITA", cita.IdCita);

                    SqlDataReader drFecha = cmdFecha.ExecuteReader();

                    while (drFecha.Read())
                    {
                        cita.Fechas.Add(Convert.ToDateTime(drFecha["FECHA_CITA"]));
                    }

                    drFecha.Close();
                }
            }

            return lista;
        }

        public bool Crear(CitaMedica obj, out string mensaje)
        {
            mensaje = "";

            try
            {
                using (SqlConnection con = new SqlConnection(cn))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_CREAR_CITA_MEDICA", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE_CITA", obj.NombreCita);
                    cmd.Parameters.AddWithValue("@DOCUMENTO_ADJUNTO", obj.DocumentoAdjunto ?? "");
                    cmd.Parameters.AddWithValue("@ID_DEPARTAMENTO", obj.IdDepartamento);
                    cmd.Parameters.AddWithValue("@ID_SUCURSAL", obj.IdSucursal);
                    cmd.Parameters.AddWithValue("@USUARIO_REGISTRO", obj.UsuarioRegistro);

                    int idCita = Convert.ToInt32(cmd.ExecuteScalar());

                    foreach (var fecha in obj.Fechas)
                    {
                        SqlCommand cmdFecha = new SqlCommand("SP_INSERTAR_FECHA_CITA", con);
                        cmdFecha.CommandType = CommandType.StoredProcedure;

                        cmdFecha.Parameters.AddWithValue("@ID_CITA", idCita);
                        cmdFecha.Parameters.AddWithValue("@FECHA_CITA", fecha);

                        cmdFecha.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool Editar(CitaMedica obj, out string mensaje)
        {
            mensaje = "";

            try
            {
                using (SqlConnection con = new SqlConnection(cn))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SP_EDITAR_CITA_MEDICA", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID_CITA", obj.IdCita);
                    cmd.Parameters.AddWithValue("@NOMBRE_CITA", obj.NombreCita);
                    cmd.Parameters.AddWithValue("@DOCUMENTO_ADJUNTO", obj.DocumentoAdjunto ?? "");
                    cmd.Parameters.AddWithValue("@ID_DEPARTAMENTO", obj.IdDepartamento);
                    cmd.Parameters.AddWithValue("@ID_SUCURSAL", obj.IdSucursal);

                    cmd.ExecuteNonQuery();

                    // 🔥 ELIMINAR FECHAS ANTERIORES
                    new SqlCommand($"DELETE FROM CITA_MEDICA_FECHAS WHERE ID_CITA = {obj.IdCita}", con).ExecuteNonQuery();

                    // 🔥 INSERTAR NUEVAS
                    foreach (var fecha in obj.Fechas)
                    {
                        SqlCommand cmdFecha = new SqlCommand("SP_INSERTAR_FECHA_CITA", con);
                        cmdFecha.CommandType = CommandType.StoredProcedure;

                        cmdFecha.Parameters.AddWithValue("@ID_CITA", obj.IdCita);
                        cmdFecha.Parameters.AddWithValue("@FECHA_CITA", fecha);

                        cmdFecha.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public CitaMedica ObtenerCitaPorId(int id)
        {
            CitaMedica cita = null;

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_CITA_POR_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_CITA", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cita = new CitaMedica
                    {
                        IdCita = Convert.ToInt32(dr["ID_CITA"]),
                        NombreCita = dr["NOMBRE_CITA"].ToString(),
                        DocumentoAdjunto = dr["DOCUMENTO_ADJUNTO"].ToString(),

                        oDepartamento = new Departamento
                        {
                            IdDepartamento = Convert.ToInt32(dr["ID_DEPARTAMENTO"]),
                            NombreDepartamento = dr["NOMBRE_DEPARTAMENTO"].ToString()
                        },

                        oSucursal = new E_Sucursales
                        {
                            IdSucursal = Convert.ToInt32(dr["ID_SUCURSAL"]),
                            NombreSucursal = dr["SUCURSAL"].ToString()
                        }
                    };
                }

                dr.Close();
            }

            return cita;
        }

        public List<DateTime> ObtenerFechas(int idCita)
        {
            List<DateTime> lista = new List<DateTime>();

            using (SqlConnection con = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SP_OBTENER_FECHAS_CITA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_CITA", idCita);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(Convert.ToDateTime(dr["FECHA_CITA"]));
                }

                dr.Close();
            }

            return lista;
        }
    }
}
