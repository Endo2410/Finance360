using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Pais { get; set; }
        public int HQID { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreContacto { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string SitioWeb { get; set; }
        public string Ruc { get; set; }
        public string NumeroTelefono { get; set; }
        public string Fax { get; set; }
        public string Terminos { get; set; }
    }
}
