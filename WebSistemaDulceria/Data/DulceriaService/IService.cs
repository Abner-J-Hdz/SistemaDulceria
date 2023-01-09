using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;
using WebSistemaDulceria.Utils;

namespace WebSistemaDulceria.Data.DulceriaService
{
    interface IService
    {
        Task<List<ProveedoresViewModel>> ObtenerProveedores();

        Task<Response> GuardarProveedor(ProveedoresViewModel proveedoresVM);

        Task<Response> ActualizarProveedor(ProveedoresViewModel proveedoresVM);

        Task<Response> EliminarProveedor(int idProveedor);

        List<ArticuloViewModel> ObtenerArticulos();
        
        
        /// Pedidos
        Task<Response> GuardarPedido(PedidoViewModel pedidoVM);

        List<PedidoViewModel> ObtenerPedidos();

    }
}
