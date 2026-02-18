using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using Capa_Entidad.Contabilidad_Alejandra;

namespace Capa_Dato.Contabilidad_Alejandra
{
    public class CD_ArchivoAdjunto
    {
        private readonly string cn = Conexion.cn;

        public int Guardar(E_ArchivoAdjunto obj)
        {
            int idGenerado = 0;

            using (SqlConnection conn = new SqlConnection(cn))
            {
                string query = @"INSERT INTO ARCHIVOS_ADJUNTOS
                    (TABLA_REFERENCIA, ID_REFERENCIA,
                     NOMBRE_ARCHIVO, NOMBRE_SISTEMA,
                     EXTENSION, RUTA_SERVIDOR, TIPO_ARCHIVO)
                    OUTPUT INSERTED.ID_ARCHIVO
                    VALUES
                    (@TablaReferencia, @IdReferencia,
                     @NombreArchivo, @NombreSistema,
                     @Extension, @RutaServidor, @TipoArchivo)
                    ";

                SqlCommand cmd = new SqlCommand(query, conn); //  usar conn

                cmd.Parameters.AddWithValue("@TablaReferencia", obj.TablaReferencia);
                cmd.Parameters.AddWithValue("@IdReferencia", obj.IdReferencia);
                cmd.Parameters.AddWithValue("@NombreArchivo", obj.NombreArchivo);
                cmd.Parameters.AddWithValue("@NombreSistema", obj.NombreSistema);
                cmd.Parameters.AddWithValue("@Extension", obj.Extension);
                cmd.Parameters.AddWithValue("@RutaServidor", obj.RutaServidor);
                cmd.Parameters.AddWithValue("@TipoArchivo", obj.TipoArchivo);


                conn.Open(); //  abrir conexión correcta
                idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return idGenerado;
        }

        public List<E_ArchivoAdjunto> Listar(string tabla, int idReferencia)
        {
            List<E_ArchivoAdjunto> lista = new List<E_ArchivoAdjunto>();

            using (SqlConnection conn = new SqlConnection(cn))
            {
                string query = @"SELECT * FROM ARCHIVOS_ADJUNTOS
                                 WHERE TABLA_REFERENCIA = @Tabla
                                 AND ID_REFERENCIA = @IdReferencia";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Tabla", tabla);
                cmd.Parameters.AddWithValue("@IdReferencia", idReferencia);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new E_ArchivoAdjunto()
                        {
                            IdArchivo = Convert.ToInt32(dr["ID_ARCHIVO"]),
                            TablaReferencia = dr["TABLA_REFERENCIA"].ToString(),
                            IdReferencia = Convert.ToInt32(dr["ID_REFERENCIA"]),
                            NombreArchivo = dr["NOMBRE_ARCHIVO"].ToString(),
                            NombreSistema = dr["NOMBRE_SISTEMA"].ToString(),
                            Extension = dr["EXTENSION"].ToString(),
                            RutaServidor = dr["RUTA_SERVIDOR"].ToString(),
                            FechaRegistro = Convert.ToDateTime(dr["FECHA_REGISTRO"])
                        });
                    }
                }
            }

            return lista;
        }

        public bool Eliminar(int idArchivo)
        {
            using (SqlConnection conn = new SqlConnection(cn))
            {
                string query = "DELETE FROM ARCHIVOS_ADJUNTOS WHERE ID_ARCHIVO = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", idArchivo);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
