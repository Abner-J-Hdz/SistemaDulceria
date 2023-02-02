using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;
using WebSistemaDulceria.Models.DulceriaModels.Graficos;

namespace WebSistemaDulceria.Data.DulceriaService
{
    interface IData
    {
        List<ArticuloViewModel> ArticulosVendidos(DateTime fechaInicio, DateTime fechaFin);

        List<ClientesMasCompras> ClientesMasCompras(DateTime fechaInicio, DateTime fechaFin);
    }
}
