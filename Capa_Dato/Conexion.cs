using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Dato
{
    public static class Conexion
    {
        public static string cn { get; }

        static Conexion()
        {
            // Crea el builder y agrega el json directamente
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            cn = configuration.GetConnectionString("CadenaSQL");
        }
    }
}
