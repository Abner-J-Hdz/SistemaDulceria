using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class ClientesViewModel
    {
        public int IdCliente { get; set; }

        [StringLength(300, ErrorMessage = "Maximo de caracteres es 300")]
        public string Nombre { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        public decimal Telefono1 { get; set; }

        public decimal Telefono2 { get; set; }

        [StringLength(1000, ErrorMessage = "Maximo de caracteres es 1000")]
        public string Direccion { get; set; }

        public bool EstaActivo { get; set; }

    }
}
