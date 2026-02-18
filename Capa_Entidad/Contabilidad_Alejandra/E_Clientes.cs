using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Contabilidad_Alejandra
{
    public class E_Clientes
    {
        public int IdCliente { get; set; }
        public int NumeroCliente { get; set; }
        public string? NombreCliente { get; set; }
        public int IdTipoServicio { get; set; }
        public string? TipoServicio { get; set; }

        public int? IdSucursal { get; set; }
        public string? Sucursal { get; set; }
        public int IdEstado { get; set; }
        public string? Estado { get; set; }
        public int IdUsuario { get; set; }
        public string? UsuarioCreador { get; set; }
        public string? UsuarioModificador { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
