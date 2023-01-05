using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class PedidoViewModel
    {

        public int IdPedido { get; set; }

        [Required(ErrorMessage = "Proveedor es requerido")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "Numero Factura es requerido")]
        public decimal? NumeroFactura { get; set; }

        public DateTime Fecha { get; set; }

        public string Observaciones { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Iva { get; set; }

        public decimal Total { get; set; }

        public bool BlockHeader { get; set; }
        public int? NumeroReferencia { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public List<DetallePedidoViewModel> DetallePedido { get; set; } = new List<DetallePedidoViewModel>();

        public ProveedoresViewModel Proveedor { get; set; } = new ProveedoresViewModel();

    }
}
