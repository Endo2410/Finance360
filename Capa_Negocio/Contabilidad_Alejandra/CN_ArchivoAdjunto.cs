using Capa_Dato.Contabilidad_Alejandra;
using Capa_Entidad.Contabilidad_Alejandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capa_Negocio.Contabilidad_Alejandra
{
    public class CN_ArchivoAdjunto
    {
        private readonly CD_ArchivoAdjunto objcd = new CD_ArchivoAdjunto();
        public int Guardar(E_ArchivoAdjunto obj)
        {
            return objcd.Guardar(obj);
        }

        public List<E_ArchivoAdjunto> Listar(string tabla, int idReferencia)
        {
            return objcd.Listar(tabla, idReferencia);
        }

        public bool Eliminar(int idArchivo)
        {
            return objcd.Eliminar(idArchivo);
        }
    }
}
