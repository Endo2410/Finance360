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


    public class Departamento
    {
        public int IdDepartamento { get; set; }
        public int IdOrigen { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string NombreDepartamento { get; set; }
    }

    public class ItemDepartamento
    {
        public int ID_DEPARTAMENTO { get; set; }
        public string NOMBRE_DEPARTAMENTO { get; set; }

        public int ID_ITEM { get; set; }
        public string DESCRIPTION { get; set; }
        public string ITEMLOOKUPCODE { get; set; }
    }
}
