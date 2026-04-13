using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad.CajaChica
{
    public class Movimiento
    {
        public int IdMovimiento { get; set; }
        public int? NumVale { get; set; }
        public string? NombresApellidos { get; set; }
        public string? Concepto { get; set; }
        public decimal Entradas { get; set; }
        public decimal Salidas { get; set; }
        public decimal RetornoDinero { get; set; }
        public string? Motivo { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoActual { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool? EsAnulado { get; set; }
        public int? IdUsuarioAutorizador { get; set; }

        public string? UsuarioCreador { get; set; }
        public string? UsuarioAutorizador { get; set; }
        public DateTime? FechaModificacion { get; set; } // Nullable porque puede no existir
        public string? MotivoAnulado { get; set; }
        public int? IdUsuarioAnulador { get; set; }
        public string? UsuarioAnulador { get; set; } // Para mostrar el nombre en el reporte

    }
}
