using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio
{
    public class CN_AlertaUsuario
    {
        private readonly CD_AlertaUsuario objcd = new CD_AlertaUsuario();

        public List<AlertaUsuario> ObtenerAlertaUsuario()
        {
            return objcd.ObtenerAlertaUsuario();
        }

        public bool GuardarAlertas(AlertaUsuario obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (obj.IdUsuario == 0)
                mensajes.Add("Debe seleccionar un usuario.");

            if (obj.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado.");

            // 🔥 SOLO VALIDAR SI VIENE CON DATOS (nuevo)
            if ((obj.TiposAlerta == null || !obj.TiposAlerta.Any()) && obj.IdUsuario != 0)
            {
                // 👉 NO bloquear → permitir borrar
            }

            if (mensajes.Any())
                return false;

            bool resultado = objcd.GuardarAlertas(obj, out string msg);

            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }



        public List<TipoAlerta> ListarTipoAlerta()
        {
            return objcd.ListarTipoAlerta();
        }

        public List<Alerta> ObtenerAlertasUsuario(int idUsuario)
        {
            return objcd.ObtenerAlertasUsuario(idUsuario);
        }

        public int ContarAlertas(int idUsuario)
        {
            return objcd.ContarAlertas(idUsuario);
        }

        public void MarcarTodasComoVistas(int idUsuario)
        {
            objcd.MarcarTodasComoVistas(idUsuario);
        }
    }
}


