using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class AjusteViewModel
    {
        public int IdAjuste { get; set; }

        public int NumeroRefencia { get; set; }

        public string Observaciones { get; set; }

        public bool EsAjusteEntrada { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public UsuarioViewModel usuario { get; set; }   

        public List<DetalleAjusteViewModel> DetalleAjuste { get; set; }
    }
}
