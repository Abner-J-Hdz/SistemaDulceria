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
        List<ArticuloViewModel> ArticulosVendidos();

        List<ClientesMasCompras> ClientesMasCompras();
    }
}
