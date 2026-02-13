using Capa_Dato;
using Capa_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class CN_PagoNC
    {
        private readonly CD_PagoNC objCD = new CD_PagoNC();

        public bool AplicarNotasCredito(List<NotaCredito> notas, out List<string> mensajes, out List<string> numerosDocumentos)
        {
            mensajes = new List<string>();
            numerosDocumentos = new List<string>();

            if (notas == null || !notas.Any())
            {
                mensajes.Add("No hay notas seleccionadas.");
                return false;
            }

            // Validar que cada nota tenga al menos un cheque
            foreach (var nota in notas)
            {
                if (nota.DetallePagos == null || !nota.DetallePagos.Any())
                    mensajes.Add($"La nota {nota.IdNC} debe tener al menos un cheque.");
            }

            if (mensajes.Any()) return false;

            // Llamar al CD para aplicar todas las notas
            bool resultado = objCD.AplicarNotasCredito(notas, out List<string> errores, out List<string> docsGenerados);

            if (!resultado)
                mensajes.AddRange(errores);

            numerosDocumentos = docsGenerados;

            return resultado;
        }
    }
    
}
