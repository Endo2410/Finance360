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
    public class CD_Permiso
    {
        private readonly string cadena = Conexion.cn;

        public List<Permiso> ObtenerEstructuraCompleta()
        {
            var lista = new List<Permiso>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_MODULOS_SUBMENU_ACCIONES", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var permiso = new Permiso
                            {
                                oModulo = new Modulo
                                {
                                    IdModulo = Convert.ToInt32(dr["IDMODULO"]),
                                    NombreModulo = dr["NOMBREMODULO"].ToString()
                                },
                                oSubMenu = dr["IDSUBMENU"] != DBNull.Value ? new SubMenu
                                {
                                    IdSubMenu = Convert.ToInt32(dr["IDSUBMENU"]),
                                    NombreSubMenu = dr["NOMBRESUBMENU"].ToString()
                                } : null,
                                oAccion = dr["IDACCION"] != DBNull.Value ? new Accion
                                {
                                    IdAccion = Convert.ToInt32(dr["IDACCION"]),
                                    NombreAccion = dr["NOMBREACCION"].ToString()
                                } : null
                            };

                            lista.Add(permiso);
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<Permiso>();
                }
            }

            return lista;
        }

        public List<Permiso> ObtenerModulosYSubMenusPorRol(int idRol)
        {
            var lista = new List<Permiso>();

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_PERMISOS_POR_ROL", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var permiso = new Permiso
                            {
                                oModulo = new Modulo
                                {
                                    IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                    NombreModulo = dr["NombreModulo"].ToString()
                                },
                                oSubMenu = dr["IdSubMenu"] != DBNull.Value ? new SubMenu
                                {
                                    IdSubMenu = Convert.ToInt32(dr["IdSubMenu"]),
                                    NombreSubMenu = dr["NombreSubMenu"].ToString()
                                } : null
                            };

                            lista.Add(permiso);
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<Permiso>();
                }
            }

            return lista;
        }

        public PermisoRolDto ObtenerPermisosPorRol(int idRol)
        {
            var result = new PermisoRolDto();

            using (SqlConnection conn = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_PERMISOS_POR_ROL", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRol", idRol);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int? idModulo = dr["IdModulo"] != DBNull.Value ? Convert.ToInt32(dr["IdModulo"]) : null;
                        int? idSubMenu = dr["IdSubMenu"] != DBNull.Value ? Convert.ToInt32(dr["IdSubMenu"]) : null;
                        int? idAccion = dr["IdAccion"] != DBNull.Value ? Convert.ToInt32(dr["IdAccion"]) : null;

                        // 1. SI tiene acción → es acción
                        if (idAccion.HasValue)
                        {
                            result.Acciones.Add(idAccion.Value);
                            continue;
                        }

                        // 2. SI tiene submenú PERO NO acción → submenú
                        if (idSubMenu.HasValue)
                        {
                            result.SubMenus.Add(idSubMenu.Value);
                            continue;
                        }

                        // 3. SI solo tiene módulo → módulo sin submenús
                        if (idModulo.HasValue)
                        {
                            result.Modulos.Add(idModulo.Value);
                        }
                    }
                }
            }

            return result;
        }


        public bool GuardarPermisos(int idRol, List<int> acciones, List<int> subMenus, List<int> modulos)
        {
            bool resultado = false;

            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GUARDAR_PERMISOS_POR_ROL", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@ListaAcciones", string.Join(",", acciones));
                    cmd.Parameters.AddWithValue("@ListaSubMenus", string.Join(",", subMenus));
                    cmd.Parameters.AddWithValue("@ListaModulos", string.Join(",", modulos));
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
                catch
                {
                    resultado = false;
                }
            }

            return resultado;
        }


        public List<Permiso> ObtenerEstructuraPorUsuario(int idUsuario)
        {
            var lista = new List<Permiso>();


            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_ESTRUCTURA_PO_USUARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var idAccion = dr["IDACCION"] != DBNull.Value ? Convert.ToInt32(dr["IDACCION"]) : (int?)null;
                        var nombreAccion = dr["NOMBREACCION"] != DBNull.Value ? dr["NOMBREACCION"].ToString() : null;

                        lista.Add(new Permiso
                        {
                            oModulo = new Modulo
                            {
                                IdModulo = Convert.ToInt32(dr["IDMODULO"]),
                                NombreModulo = dr["NOMBREMODULO"].ToString()
                            },
                            oSubMenu = dr["IDSUBMENU"] != DBNull.Value ? new SubMenu
                            {
                                IdSubMenu = Convert.ToInt32(dr["IDSUBMENU"]),
                                NombreSubMenu = dr["NOMBRESUBMENU"].ToString()
                            } : null
                        });
                    }
                }
            }

            return lista;
        }


        public List<string> ObtenerAccionesPorUsuario(int idUsuario)
        {
            var lista = new List<string>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_ACCIONES_POR_USUARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(dr["NOMBREACCION"].ToString().Trim());
                    }
                }
            }

            return lista;
        }

    }
}
