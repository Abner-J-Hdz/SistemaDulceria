﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class DetalleProductoTerminadoViewModel
    {
        public int IdArticuloTerminado { get; set; }

        public int IdArticuloMaterial { get; set; }

        public int Cantidad { get; set; }
    }
}
