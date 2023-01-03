using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;

namespace WebSistemaDulceria.Data.DulceriaService
{
    public class Data
    {
        private readonly DbContextDulceria context;
        /*public Data(DbContextDulceria _context)
        {
            context = _context;
        }*/
        public List<ArticuloViewModel> ObtenerArticulos()
        {
            try
            {
                var ArticulosDb = context.Articulo.Where(x => x.EstaActivo).ToList();

                return ArticulosDb.Select(x => new ArticuloViewModel
                {
                    IdArticulo = x.IdArticulo,
                    Nombre = x.Nombre,
                    CodInterno = x.CodInterno,
                    CodBarra = x.CodBarra,
                    IdPresentacion = x.IdPresentacion,
                    IdUnidadMedida = x.IdUnidadMedida,
                    CantidadMinima = x.CantidadMinima,
                    TieneVencimiento = x.TieneVencimiento,
                    EsMenudeo = x.EsMenudeo,
                    EsProductoTerminado = x.EsProductoTerminado,
                    EstaActivo = x.EstaActivo
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
