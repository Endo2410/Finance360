using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class CampaniaPublicitaria
    {
        public int IdCampania { get; set; }
        public string NombreCampania { get; set; }
        public string NumeroCampania { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string UsuarioRegistro { get; set; }


        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public int IdProveedor { get; set; }
        public int? IdDepartamento { get; set; }
        public int IdTipoPublicidad { get; set; }
        public int IdModalidad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoInversion { get; set; }
        public decimal Saldovencido { get; set; }
        public int IdPais { get; set; }
        public int IdMoneda { get; set; }
        public int IdEstado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public decimal TotalRetenciones { get; set; }
        public decimal TotalNeto { get; set; }

        public Proveedor oProveedor { get; set; }
        public Departamento ODepartamento { get; set; }
        public TipoPublicidad oTipoPublicidad { get; set; }
        public Modalidad oModalidad { get; set; }
        public Pais oPais { get; set; }
        public Moneda oMoneda { get; set; }

        public Estado oEstado { get; set; }



        //Nueva para estado 
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoPago { get; set; } // Esto tomará EP.NOMBRE
        public string EstadoCampania { get; set; } // Esto tomará EC.NOMBRE


        public int IdEstadoPago { get; set; }
        public Estado oEstadoPago { get; set; }


    }
}


