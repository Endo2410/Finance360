using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.Contabilidad_Alejandra
{
    public class E_CxP_Contabilidad_Alejandra
    {
        public int IdCxP { get; set; }
        public int IdCliente { get; set; }
        public int IdMesxPagar { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoxPagar { get; set; }
        public string? Observaciones { get; set; }
        public int IdEstado { get; set; }

        public int IdUsuario { get; set; }

        // Campos visualización
        public string? NumeroCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? TipoServicio { get; set; }
        public string? Sucursal { get; set; }
        public string? MesDescripcion { get; set; }
        public string? Estado { get; set; }
        public string? UsuarioCreador { get; set; }
        public string? UsuarioModificador { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool TieneReciboPendiente { get; set; }
        public bool TieneReciboPagado { get; set; }

        public string? RutaReciboPendiente { get; set; }
        public string? RutaReciboPagado { get; set; }

    }
}
