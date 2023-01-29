using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Ajuste
    {
        [Key]
        public int IdAjuste { get; set; }

        public int NumeroRefencia { get; set; }

        public string Observaciones { get; set; }

        public bool EsAjusteEntrada { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public ICollection<DetalleAjuste> DetalleAjuste { get; set; }
    }
}
