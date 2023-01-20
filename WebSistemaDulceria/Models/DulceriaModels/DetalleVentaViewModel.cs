using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class DetalleVentaViewModel
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

        public bool Editable { get; set; }

        public VentaViewModel Venta { get; set; }

        public ArticuloViewModel Articulo { get; set; } = new ArticuloViewModel();
    }
}
