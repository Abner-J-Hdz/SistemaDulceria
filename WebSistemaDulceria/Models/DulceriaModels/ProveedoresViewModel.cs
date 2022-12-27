﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class ProveedoresViewModel
    {
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [StringLength(300, ErrorMessage = "El nombre no deber tener mas de 300 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Correo es requerido")]
        [EmailAddress(ErrorMessage = "Escribe un correo valido")]
        [StringLength(100, ErrorMessage = "El nombre no deber tener mas de 100 caracteres")]
        public string Correo { get; set; }

       
        public decimal Telefono1 { get; set; }


        public decimal Telefono2 { get; set; }

        [StringLength(1000, ErrorMessage = "El nombre no deber tener mas de 1000 caracteres")]
        public string Direccion { get; set; }

        [StringLength(30)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Ruc { get; set; }

        public bool EstaActivo { get; set; }

        //public DateTime FechaCreacion { get; set; }

        //public int IdUsuarioCreacion { get; set; }

        //public DateTime FechaModificacion { get; set; }

        //public int IdUsuarioModificacion { get; set; }
    }
}
