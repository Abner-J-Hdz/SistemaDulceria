﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Models.DulceriaModels
{
    public class ArticuloViewModel
    {
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

        [StringLength(5, ErrorMessage = "El CodBarra no deber tener mas de 5 caracteres")]
        [RegularExpression("^[0-9]{5}$",ErrorMessage = "Solo numero de menos de 5 digitos")]
        public string CodBarra { get; set; }

        public bool TieneVencimiento { get; set; }

        public bool EsMenudeo { get; set; }

        public bool CantidadMenudeo { get; set; }

        public bool EsProductoTerminado { get; set; }

        public bool EstaActivo { get; set; }

        public List<PreciosViewModel> Precios { get; set; } = new List<PreciosViewModel>();

        public List<LoteViewModel> Lote { get; set; } = new List<LoteViewModel>();

        public List<DetalleProductoTerminadoViewModel> DetalleProductoTerminado { get; set; } = new List<DetalleProductoTerminadoViewModel>();

        public decimal PrecioInicial { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }

        /*public DateTime FechaCreacion { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }*/
    }
}
