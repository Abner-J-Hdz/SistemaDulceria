using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class UsuarioViewModel
    {

        [Key]
        public int IdUsuario { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El nombre no deber tener mas de 100 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El Apellido no deber tener mas de 100 caracteres")]
        public string Apellido { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El Email no deber tener mas de 100 caracteres")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string Email { get; set; }

        public string Password { get; set; }

        public bool EstaActivo { get; set; }
    }
}
