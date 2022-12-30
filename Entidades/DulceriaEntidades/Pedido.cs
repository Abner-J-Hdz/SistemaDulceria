using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "Proveedor es requerido")]
        public int IdProveedor { get; set; }

        public decimal NumeroFactura { get; set; }

        public DateTime Fecha { get; set; }

        public string Observaciones { get; set; }

        public  decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Iva { get; set; }

        public decimal Total { get; set; }

        public int NumeroReferencia { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public ICollection<DetallePedido> DetallePedido { get; set; }

    }
}
