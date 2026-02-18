using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//estas 3 referencias nos ayudaran a mandar un mail al usuario
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Capa_Negocio
{
    public class CN_Recursos
    {
        //aca generamos la clave
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;

        }

        //encriptar la clave
        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            //usar la referencia de "System.security.cryptography"
            //encriptamos la clave
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));


                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));

                }

            }
            return sb.ToString();
        }


        // Método para enviar correo usando servidor interno
        public static bool EnviarCorreoInterno(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                // Validar formato de correo
                if (string.IsNullOrWhiteSpace(correo) || !EsCorreoValido(correo))
                {
                    Console.WriteLine($"Correo no válido: {correo}");
                    return false;
                }

                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("soportetecnico@farmaciasaba.com", "Sistema de Soporte Saba Nicaragua");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Host = "192.168.222.17",
                    Port = 25,
                    EnableSsl = false,
                    Credentials = new NetworkCredential("soportetecnico@farmaciasaba.com", "Fsaba2014")
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error de formato en el correo: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enviando correo: " + ex.Message);
            }

            return resultado;
        }
        private static bool EsCorreoValido(string correo)
        {
            try
            {
                var addr = new MailAddress(correo);
                return addr.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        ///enviar alertas
        
    }
}