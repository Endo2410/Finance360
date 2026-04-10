using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class ComprasCliente
    {
        public string Farmacia { get; set; }
        public int StoreID { get; set; }
        public DateTime Time { get; set; }
        public string TransactionNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Nombre { get; set; }
        public decimal Total { get; set; }
    }
}
