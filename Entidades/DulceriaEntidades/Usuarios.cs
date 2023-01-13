using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Usuarios
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
        public string Email { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "El Password no deber tener mas de 100 caracteres")]
        public string Password { get; set; }
    }
}
