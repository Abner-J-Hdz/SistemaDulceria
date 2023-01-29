using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class DetalleAjusteViewModel
    {
        public int IdAjuste { get; set; }

        public int IdDetalleAjuste { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public int IdArticulo { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Costo { get; set; }

        public decimal TotalDetalleAjuste { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public ArticuloViewModel Articulo { get; set; } = new ArticuloViewModel();
    }
}
