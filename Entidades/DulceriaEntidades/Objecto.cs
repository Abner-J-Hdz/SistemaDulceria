﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidades.DulceriaEntidades
{
    public class Objecto
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
    }
}