using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class OrdenCompra
    {
        public string Proveedor { get; set; }
        //public string NumeroOrden { get; set; }
        public string Estado { get; set; }
        public string Confirmacion { get; set; }
        public string Observaciones { get; set; }
        //public DateTime FechaCreacion { get; set; }


        //Nuevo
        public int IdOrden { get; set; }
        public int IdOrdenCompra { get; set; }  // ID real de FSCDBd
        public int HQID { get; set; }
        public string NumeroOrden { get; set; }
        public string StatusOrden { get; set; }
        public string Factura { get; set; }
        public string NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }
        public int IdEstado { get; set; }
        public string Comentario { get; set; }
        public int? IdUsuarioPago { get; set; }
        public DateTime? FechaPago { get; set; }
    }

    public class ReporteCompras
    {
        public string Proveedor { get; set; }
        public string Laboratorio { get; set; }
        public decimal TotalComprado { get; set; }
    }

    public class OrdenSinRecibir
    {
        public string PONumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string Farmacia { get; set; }
    }


    public class PagoFactura
    {

        public int IdProveedor { get; set; }

        public string NumeroCheque { get; set; }

        public int IdBanco { get; set; }

        public DateTime Fecha { get; set; }

        public decimal MontoCheque { get; set; }

        //public List<DetalleFacturaPago> Facturas { get; set; }

        //public List<DetalleNotaCreditoPago> NotasCredito { get; set; }

    }
}
