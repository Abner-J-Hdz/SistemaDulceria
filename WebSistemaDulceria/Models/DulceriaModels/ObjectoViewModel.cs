using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class ObjectoViewModel
    {
        public int IdObjeto { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "Maximo carateres 5")]
        public string CodigoInterno { get; set; }
        public string Nombre { get; set; }
        public int IdTipoObjeto { get; set; }
        public bool EstaActivo { get; set; }

        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdUsuarioModificacion { get; set; }

        public TipoObjectoViewModel TipoObjecto { get; set; } = new TipoObjectoViewModel();
    }
}
