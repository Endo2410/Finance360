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

        public bool CrearAlertaUsuario(AlertaUsuario obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (obj.IdUsuario == 0)
                mensajes.Add("Debe seleccionar un usuario.");

            if (obj.IdTipoAlerta == 0)
                mensajes.Add("Debe seleccionar un tipo de alerta.");

            if (obj.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado.");

            if (mensajes.Any())
                return false;

            bool resultado = objcd.CrearAlertaUsuario(obj, out string msg);

            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }

        public bool EditarAlertaUsuario(AlertaUsuario obj, out List<string> mensajes)
        {
            mensajes = new List<string>();

            if (obj.IdUsuario == 0)
                mensajes.Add("Debe seleccionar un usuario.");

            if (obj.IdTipoAlerta == 0)
                mensajes.Add("Debe seleccionar un tipo de alerta.");

            if (obj.IdEstado == 0)
                mensajes.Add("Debe seleccionar un estado.");

            if (mensajes.Any())
                return false;

            bool resultado = objcd.EditarAlertaUsuario(obj, out string msg);

            if (!resultado)
                mensajes.Add(msg);

            return resultado;
        }

        public List<TipoAlerta> ListarTipoAlerta()
        {
            return objcd.ListarTipoAlerta();
        }
    }
}


