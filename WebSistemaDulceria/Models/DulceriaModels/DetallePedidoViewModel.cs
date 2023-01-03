using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class DetallePedidoViewModel
    {
        public int IdDetallePedido { get; set; }

        public int IdPedido { get; set; }

        public int IdArticulo { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Iva { get; set; }

        public decimal Total { get; set; }

        public ArticuloViewModel Articulo { get; set; } = new ArticuloViewModel(); 

    }
}
