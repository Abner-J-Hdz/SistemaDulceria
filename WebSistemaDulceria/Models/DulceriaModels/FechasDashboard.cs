using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class FechasDashboard
    {
        public DateTime fechaInicio { get; set; } = DateTime.Now;
        public DateTime fechaFin { get; set; } = DateTime.Now;
    }
}
