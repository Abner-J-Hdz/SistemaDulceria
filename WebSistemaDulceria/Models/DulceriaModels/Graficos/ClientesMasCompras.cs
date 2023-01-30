using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels.Graficos
{
    public class ClientesMasCompras
    {
        public string Nombre { get; set; }

        public string Correo { get; set; }

        public decimal Total { get; set; }

        public int NumeroVentas { get; set; }
    }
}
