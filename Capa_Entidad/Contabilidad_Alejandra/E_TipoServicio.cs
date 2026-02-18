using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Contabilidad_Alejandra
{
    public class E_TipoServicio
    {
        public int IdTipoServicio { get; set; }
        public string? DescripcionServicio { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioCreador { get; set; }
        public string? UsuarioModificador { get; set; }
    }
}
