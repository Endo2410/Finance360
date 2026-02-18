using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class AcuerdoRebate
    {
        public int IdAcuerdo { get; set; }
        public string NumeroAcuerdo { get; set; }
        public string NombreAcuerdo { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Documento { get; set; }

        public int IdProveedor { get; set; }
        public int? IdDepartamento { get; set; }
        public int IdModalidadOp { get; set; }
        public int IdTipoRebate { get; set; }
        public int IdCriterio { get; set; }

        public decimal ValorCriterio { get; set; }
        public decimal Ganancia { get; set; }

        public int IdPais { get; set; }
        public int IdMoneda { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public int IdEstado { get; set; }
        public string Comentario { get; set; }

        public Proveedor oProveedor { get; set; }
        public Departamento ODepartamento { get; set; }
        public ModalidadOperacion oModalidadOperacion { get; set; }
        public TipoRebate oTipoRebate { get; set; }
        public CriterioRebate oCriterio { get; set; }
        public Pais oPais { get; set; }
        public Moneda oMoneda { get; set; }
        public Estado oEstado { get; set; }
    }
}
