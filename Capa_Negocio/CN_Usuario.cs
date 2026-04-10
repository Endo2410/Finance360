using Capa_Dato;
using Capa_Entidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_Usuario
    {
        private readonly CD_Usuario objcd = new CD_Usuario();

        public List<Usuario> ObtenerUsuarios()
        {
            return objcd.ObtenerUsuarios();
        }

        public bool CrearUsuario(Usuario obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            // Validaciones
            if (string.IsNullOrWhiteSpace(obj.Nombres))
                mensajes.Add("El campo Nombres es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombres, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("Los nombres solo pueden contener letras.");

            if (string.IsNullOrWhiteSpace(obj.Apellidos))
                mensajes.Add("El campo Apellidos es obligatorio.");
            else if (!Regex.IsMatch(obj.Apellidos, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("Los apellidos solo pueden contener letras.");

            if (string.IsNullOrWhiteSpace(obj.NombreUsuario))
                mensajes.Add("El campo Usuario es obligatorio.");
            else if (obj.NombreUsuario.Contains(" "))
                mensajes.Add("El nombre de usuario no puede contener espacios.");

            if (string.IsNullOrWhiteSpace(obj.Correo))
                mensajes.Add("El campo Correo es obligatorio.");
            else if (!Regex.IsMatch(obj.Correo, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                mensajes.Add("El correo no tiene un formato válido.");

            // Verificar duplicados
            var listaUsuarios = objcd.ObtenerUsuarios();
            if (listaUsuarios.Any(u => u.NombreUsuario.Equals(obj.NombreUsuario, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El nombre de usuario ya existe.");
            if (listaUsuarios.Any(u => u.Correo.Equals(obj.Correo, StringComparison.OrdinalIgnoreCase)))
                mensajes.Add("El correo ya existe.");

            if (mensajes.Any())
                return false;

            // Generar clave
            string claveGenerada = CN_Recursos.GenerarClave();
            obj.Clave = CN_Recursos.ConvertirSha256(claveGenerada);
            obj.Reestablecer = true;

            bool resultado = objcd.CrearUsuario(obj, out string msg);

            if (resultado)
            {
                CN_Recursos.EnviarCorreoInterno(obj.Correo, "Credenciales de acceso",
                    $"Usuario: <b>{obj.NombreUsuario}</b><br/>Contraseña temporal: <b>{claveGenerada}</b>");
                mensajes.Add("Usuario creado correctamente y correo enviado.");
            }
            else
            {
                mensajes.Add(msg);
            }

            return resultado;
        }

        public bool EditarUsuario(Usuario obj, out List<string> mensajes)
        {
            mensajes = new List<string>();


            // Validaciones
            if (string.IsNullOrWhiteSpace(obj.Nombres))
                mensajes.Add("El campo Nombres es obligatorio.");
            else if (!Regex.IsMatch(obj.Nombres, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("Los nombres solo pueden contener letras.");

            if (string.IsNullOrWhiteSpace(obj.Apellidos))
                mensajes.Add("El campo Apellidos es obligatorio.");
            else if (!Regex.IsMatch(obj.Apellidos, @"^[a-zA-ZÁÉÍÓÚáéíóúÑñ\s]+$"))
                mensajes.Add("Los apellidos solo pueden contener letras.");

            if (string.IsNullOrWhiteSpace(obj.NombreUsuario))
                mensajes.Add("El campo Usuario es obligatorio.");
            else if (obj.NombreUsuario.Contains(" "))
                mensajes.Add("El nombre de usuario no puede contener espacios.");

            if (string.IsNullOrWhiteSpace(obj.Correo))
                mensajes.Add("El campo Correo es obligatorio.");
            else if (!Regex.IsMatch(obj.Correo, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                mensajes.Add("El correo no tiene un formato válido.");

            // Verificar duplicados, ignorando el usuario actual
            var listaUsuarios = objcd.ObtenerUsuarios();
            if (listaUsuarios.Any(u => u.NombreUsuario.Equals(obj.NombreUsuario, StringComparison.OrdinalIgnoreCase) && u.IdUsuario != obj.IdUsuario))
                mensajes.Add("El nombre de usuario ya existe.");
            if (listaUsuarios.Any(u => u.Correo.Equals(obj.Correo, StringComparison.OrdinalIgnoreCase) && u.IdUsuario != obj.IdUsuario))
                mensajes.Add("El correo ya existe.");

            if (mensajes.Any())
                return false;

            // Editar usuario en BD
            bool resultado = objcd.EditarUsuario(obj, out string msg);
            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }



        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            try
            {
                return objcd.CambiarClave(idusuario, nuevaclave, out Mensaje);
            }
            catch (Exception ex)
            {
                Mensaje = "Error al cambiar la clave: " + ex.Message;
                return false;
            }
        }

        public bool RestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            try
            {
                Mensaje = string.Empty;
                string nuevaclave = CN_Recursos.GenerarClave();
                bool resultado = objcd.RestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

                if (resultado)
                {
                    string asunto = "Contraseña Restablecida";
                    string mensaje_correo = " <h3> su cuenta fue restablecida correctamente  </h3> </br> <p>Su contraseña para acceder ahora es :!clave!</p>";
                    mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                    bool respuesta = CN_Recursos.EnviarCorreoInterno(correo, asunto, mensaje_correo);

                    if (respuesta)
                    {
                        return true;
                    }
                    else
                    {
                        Mensaje = "No se pudo enviar el correo";
                        return false;
                    }
                }
                else
                {
                    Mensaje = "No se pudo restablecer la contraseña";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al restablecer la clave: " + ex.Message;
                return false;
            }
        }


        public List<Usuario> UsuariosPorRol(int idRol)
        {
            return objcd.UsuariosPorRol(idRol);
        }

    }
}
