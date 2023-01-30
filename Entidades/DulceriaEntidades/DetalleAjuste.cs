using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class DetalleAjuste
    {
        public int IdAjuste { get; set; }

        [Key]
        public int IdDetalleAjuste { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public int IdArticuloMaterial { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Costo { get; set; }

        public decimal Total {get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModficacion { get; set; }

        public Ajuste Ajuste { get; set; }

        public Articulo Articulo { get; set; }

    }
}
