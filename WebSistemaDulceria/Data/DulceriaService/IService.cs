﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;
using WebSistemaDulceria.Utils;

namespace WebSistemaDulceria.Data.DulceriaService
{
    interface IService
    {

        #region Proveedores
        Task<List<ProveedoresViewModel>> ObtenerProveedores();

        Task<Response> GuardarProveedor(ProveedoresViewModel proveedoresVM);

        Task<Response> ActualizarProveedor(ProveedoresViewModel proveedoresVM);

        Task<Response> EliminarProveedor(int idProveedor);

        #endregion

        #region Articulos
        Task<Response> GuardarArticulo(ArticuloViewModel articuloVM);

        Task<Response> ActualizarArticulo(ArticuloViewModel articuloVM);

        List<ArticuloViewModel> ObtenerArticulos();

        Task<Response> EliminarArticulo(int idArticulo);

        //List<ArticuloViewModel> ArticulosVendidos();
        #endregion

        #region Pedido
        Task<Response> GuardarPedido(PedidoViewModel pedidoVM);

        List<PedidoViewModel> ObtenerPedidos();

        #endregion

        #region Clientes
        List<ClientesViewModel> ObtenerClientes();

        Task<Response> GuardarClientes(ClientesViewModel clienteVM);

        Task<Response> ActualizarClientes(ClientesViewModel clienteVM);

        Task<Response> EliminarClientes(int idCliente);

        #endregion

        #region Ventas

        Task<Response> GuardarVentas(VentaViewModel ventaVM);

        List<VentaViewModel> ObtenerVentas();

        Task<VentaViewModel> ObtenerVenta(int IdVenta);

        Task<Response> EliminarVenta(int IdVenta);

        #endregion

        #region Ajuste de inventario
        Task<Response> GuardarAjusteInventario(AjusteViewModel ajuste);

        List<AjusteViewModel> ObtenerAjustesinventario();

        AjusteViewModel ObtenerUnAjusteInventario(int Id);

        Task<Response> EliminarAjusteInventario(int IdAjuste);

        #endregion

        #region Usuarios
        Task<Response> GuardarUsuario(UsuarioViewModel usuarioVM);

        List<UsuarioViewModel> ObtenerUsuario();

        Task<Response> ActualizarUsuario(UsuarioViewModel usuarioVM);

        Task<Response> ActualizarPassword(int IdUsuario, string password);

        Task<Response> EliminarUsuario(int IdUsuario);

        Response GetUsuarioLogin(string email, string password);

        Task<Response> CambiarContraseña(string email, string password);
        #endregion

    }
}
