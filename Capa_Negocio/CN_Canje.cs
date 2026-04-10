using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio
{
    public class CN_Canje
    {
        private readonly CD_Canje cd = new CD_Canje();

        public List<Canje> ObtenerCanjes() => cd.ObtenerCanjes();

        public bool Crear(Canje obj, out List<string> mensajes)
        {
            mensajes = new();


            if (obj.Volumen <= 0)
                mensajes.Add("El volumen debe ser mayor a cero.");

            if (obj.Monto <= 0)
                mensajes.Add("El monto debe ser mayor a cero.");

            if (obj.IdProveedor <= 0)
                mensajes.Add("Debe seleccionar un proveedor.");

            if (obj.IdDepartamento == null || obj.IdDepartamento <= 0)
                mensajes.Add("Debe seleccionar un Departamento.");


            if (obj.IdTipoCanje <= 0)
                mensajes.Add("Debe seleccionar un tipo de canje.");

            if (string.IsNullOrEmpty(obj.DocumentoAdjunto))
                mensajes.Add("Debe adjuntar un documento.");

            //if (string.IsNullOrEmpty(obj.ArchivoActa))
            //    mensajes.Add("Debe adjuntar una Acta.");

            if (mensajes.Any()) return false;

            bool ok = cd.Crear(obj, out string msg);
            if (!ok) mensajes.Add(msg);

            return ok;
        }

        public bool Editar(Canje obj, out List<string> mensajes)
        {
            mensajes = new();

            if (obj.IdCanje <= 0)
                mensajes.Add("Canje inválido.");

            if (mensajes.Any()) return false;

            bool ok = cd.Editar(obj, out string msg);
            if (!ok) mensajes.Add(msg);

            return ok;
        }

        public List<Canje> ObtenerCanjesresumen()
        {
            return cd.ObtenerCanjesresumen();
        }

        public List<DetallePagoCanje1> ObtenerDetallePagoCanje(int idCanje)
        {
            return cd.ObtenerDetallePagoCanje(idCanje);
        }

        public bool AnularPagoCanje(int idDetallePago, string usuario)
        {
            if (idDetallePago <= 0)
                return false;

            return cd.AnularPagoCanje(idDetallePago, usuario);
        }
    }
}
