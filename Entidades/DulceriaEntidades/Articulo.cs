using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Articulo
    {
        [Key]
        public int IdArticulo { get; set; }

        [Required(ErrorMessage = "Nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no deber tener mas de 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "CodInterno es obligatorio")]
        [StringLength(5, ErrorMessage = "El nombre no deber tener mas de 5 caracteres")]
        public string CodInterno { get; set; }

        public int IdPresentacion { get; set; }

        public int IdUnidadMedida { get; set; }

        public int CantidadMinima { get; set; }

        public string CodBarra { get; set; }

        public bool TieneVencimiento { get; set; }

        public bool EsMenudeo { get; set; }

        public bool CantidadMenudeo { get; set; }

        public bool EsProductoTerminado { get; set; }
        
        public bool EstaActivo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }
    }
}
