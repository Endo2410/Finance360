using Capa_Dato.CajaChica;
using Capa_Entidad.CajaChica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.CajaChica
{
    public class CN_Cheques
    {
        private CD_Cheques objCapaDato = new CD_Cheques();
        public List<Cheque> Listar()
        {
            return objCapaDato.Listar();
        }
        public bool Registrar(Cheque obj, out string mensaje)
        {
            // Validaciones básicas de negocio
            if (obj.NumeroCheque <= 0)
            {
                mensaje = "El número de cheque debe ser válido.";
                return false;
            }
            if (string.IsNullOrEmpty(obj.Concepto))
            {
                mensaje = "El concepto no puede estar vacío.";
                return false;
            }

            return objCapaDato.Registrar(obj, out mensaje);
        }
    }
}
