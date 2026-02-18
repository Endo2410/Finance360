using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Contabilidad_Alejandra
{
    public class E_ArchivoAdjunto
    {
        public int IdArchivo { get; set; }

        public string? TablaReferencia { get; set; }
        public int IdReferencia { get; set; }

        public string? NombreArchivo { get; set; }
        public string? NombreSistema { get; set; }
        public string? Extension { get; set; }
        public string? RutaServidor { get; set; }

        public DateTime FechaRegistro { get; set; }
        public string? TipoArchivo { get; set; }

    }
}
