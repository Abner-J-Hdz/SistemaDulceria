using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class DetalleProductoTerminado
    {
        public int IdArticuloTerminado { get; set; }

        public int IdArticuloMaterial { get; set; }

        public int Cantidad { get; set; }
    }
}
