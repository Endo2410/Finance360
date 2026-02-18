using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class AlertaUsuario
    {
        public int IdAlertaUsuario { get; set; }

        public int IdTipoAlerta { get; set; }
        public TipoAlerta oTipoAlerta { get; set; }

        public int IdUsuario { get; set; }
        public Usuario oUsuario { get; set; }

        public int IdEstado { get; set; }
        public Estado oEstado { get; set; }
    }


    public class TipoAlerta
    {
        public int IdTipoAlerta { get; set; }
        public string Codigo { get; set; }
    }

    public class Alerta
    {
        public int IdAlerta { get; set; }
        public int IdTipoAlerta { get; set; }
        public int IdReferencia { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaAlerta { get; set; }
        public bool Enviada { get; set; }
    }
}
