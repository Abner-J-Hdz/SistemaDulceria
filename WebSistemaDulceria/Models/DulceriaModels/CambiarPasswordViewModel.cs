using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class CambiarPasswordViewModel
    {
        public int IdUsuario { get; set; }

        
        [Required]
        [StringLength(15, ErrorMessage = "Maximo de carateres 15")]
        public string Contrasena { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Maximo de carateres 15")]
        public string RepiteContrasena { get; set; }

    }
}
