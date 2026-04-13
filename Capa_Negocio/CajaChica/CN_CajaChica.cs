using Capa_Dato.CajaChica;
using Capa_Entidad.CajaChica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio.CajaChica
{
    public class CN_CajaChica
    {
        // Instancia de la capa de datos
        private CD_CajaChica objCapaDato = new CD_CajaChica();
        public List<Movimiento> Listar()
        {
            return objCapaDato.Listar();
        }
        public int RegistrarMovimiento(Movimiento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // --- REGLAS DE NEGOCIO (Validaciones) ---

            if (string.IsNullOrEmpty(obj.NombresApellidos) || string.IsNullOrWhiteSpace(obj.NombresApellidos))
            {
                Mensaje = "Debe indicar el nombre del beneficiario.";
            }
            else if (string.IsNullOrEmpty(obj.Concepto) || string.IsNullOrWhiteSpace(obj.Concepto))
            {
                Mensaje = "El concepto del movimiento no puede estar vacío.";
            }
            else if (obj.Entradas < 0 || obj.Salidas < 0)
            {
                Mensaje = "Los montos de entrada o salida no pueden ser negativos.";
            }
            else if (obj.Entradas == 0 && obj.Salidas == 0)
            {
                Mensaje = "Debe registrar un monto mayor a cero en entradas o salidas.";
            }
            else if (obj.IdUsuario == 0)
            {
                Mensaje = "Error: No se ha identificado al usuario que realiza la operación.";
            }

            // Si hay un mensaje de error, retornamos 0 y no llamamos a la capa de datos
            if (!string.IsNullOrEmpty(Mensaje))
            {
                return 0;
            }
            else
            {
                // Si todo está correcto, procedemos a registrar en la DB
                return objCapaDato.RegistrarMovimiento(obj, out Mensaje);
            }
        }

    
        public bool AnularMovimiento(int idMovimiento, string motivo, int idUsuarioAnulador, out string mensaje)
        {
            if (string.IsNullOrWhiteSpace(motivo))
            {
                mensaje = "Debe proporcionar un motivo para la anulación.";
                return false;
            }
            // Pasamos el idUsuarioAnulador a la capa de datos
            return objCapaDato.AnularMovimiento(idMovimiento, motivo, idUsuarioAnulador, out mensaje);
        }
        public bool AplicarRetorno(int idMovimiento, decimal monto, string motivo, out string mensaje)
        {
            mensaje = string.Empty;

            // --- REGLAS DE NEGOCIO ---

            // 1. Validar que el ID sea válido
            if (idMovimiento <= 0)
            {
                mensaje = "El identificador del movimiento no es válido.";
                return false;
            }

            // 2. Validar que el monto sea positivo
            if (monto <= 0)
            {
                mensaje = "El monto de retorno debe ser mayor a cero.";
                return false;
            }

            // 3. Validar el motivo
            if (string.IsNullOrWhiteSpace(motivo))
            {
                mensaje = "Debe proporcionar un motivo para el retorno de efectivo.";
                return false;
            }

            if (motivo.Length < 5)
            {
                mensaje = "El motivo es demasiado corto. Por favor, sea más específico.";
                return false;
            }

            // Si pasa todas las validaciones, enviamos a la Capa de Datos
            return objCapaDato.AplicarRetorno(idMovimiento, monto, motivo, out mensaje);
        }
    }
}
