using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Entidad.Contabilidad_Alejandra;


namespace Capa_Entidad
{
    public class Incentivo
    {
        public int IdIncentivo { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }

        public int IdSucursal { get; set; }
        public int IdProveedor { get; set; }
        public int IdTipoCanje { get; set; }
        public int IdTipoIncentivo { get; set; }

        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string DocumentoAdjunto { get; set; }
        public string Comentario { get; set; }

        public E_Sucursales oSucursal { get; set; }
        public Proveedor oProveedor { get; set; }
        public TipoCanje oTipoCanje { get; set; }
        public TipoIncentivo oTipoIncentivo { get; set; }
        public Estado oEstado { get; set; }
    }

    public class PagoIncentivo
    {
        public int IdIncentivo { get; set; }

        public decimal MontoTotal { get; set; }

        public string Observacion { get; set; }

        public List<DetallePagoIncentivo> Detalles { get; set; }
    }

    public class DetallePagoIncentivo
    {
        public int IdTipoDocumento { get; set; }

        public decimal MontoPagado { get; set; }

        public string NumeroConfirmacion { get; set; }

        public string RutaComprobante { get; set; }

        public string UsuarioPago { get; set; }
    }

    public class IncentivoSaldo
    {
        public decimal TotalEntrada { get; set; }

        public decimal TotalSalida { get; set; }

        public decimal SaldoDisponible { get; set; }
    }

    public class IncentivoRecibido
    {
        public int IdPagoIncentivo { get; set; }

        public DateTime Fecha { get; set; }

        public string DocumentoPago { get; set; }

        public string DocumentoIncentivo { get; set; }

        public string Incentivo { get; set; }

        public string Proveedor { get; set; }

        public decimal Monto { get; set; }
    }

    public class IncentivoMovimiento
    {
        public int IdMovimiento { get; set; }
        public int IdSucursal { get; set; }
        public int IdTipoUso { get; set; }
        public decimal Monto { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Observacion { get; set; }
        public string Comprobante { get; set; }
        public string TipoUsoNombre { get; set; }
        public DateTime FechaMovimiento { get; set; }
                        
    }
}
