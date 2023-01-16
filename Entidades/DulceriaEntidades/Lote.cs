using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Lote
    {
        public int IdLote { get; set; }

        public int IdArticulo { get; set; }

        public int Cantidad { get; set; }

        public Articulo Articulo { get; set; }
    }
}
