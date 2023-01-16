using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Precios
    {
        [Key]
        public int IdArticuloPrecio { get; set; }

        public int IdArticulo { get; set; }

        public decimal PrecioInicial { get; set; }

        public decimal PrecioCosto { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no deber tener mas de 5 caracteres")]
        public string MargenGanancia { get; set; }

        public decimal PrecioVenta { get; set; }

        public Articulo Articulo { get; set; }

    }
}
