using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class VentaViewModel
    {
        public int IdVenta { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public bool EstaActiva { get; set; }

        [Required]
        public decimal NumeroRecibo { get; set; }

        [Required]
        public decimal SubTotal { get; set; }

        [Required]
        public decimal Descuento { get; set; }

        [Required]
        public decimal Iva { get; set; }

        [Required]
        public decimal Total { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        [StringLength(250, ErrorMessage = "Máximo de caracteres 250")]
        public string Observaciones { get; set; }

        public ClientesViewModel Cliente { get; set; } = new ClientesViewModel();

        public List<DetalleVentaViewModel> DetalleVenta { get; set; } = new List<DetalleVentaViewModel>();
    }
}
