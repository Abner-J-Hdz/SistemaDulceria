﻿using Datos;
using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSistemaDulceria.Models.DulceriaModels;
using WebSistemaDulceria.Utils;

namespace WebSistemaDulceria.Data.DulceriaService
{
    public class Service : IService
    {
        private readonly DbContextDulceria context;
        public Service(DbContextDulceria _context)
        {
            context = _context;
        }

        #region Proveedores

        public async Task<List<ProveedoresViewModel>> ObtenerProveedores()
        {
            try
            {
                var proveedoresDb = await context.Proveedores.ToListAsync();

                return proveedoresDb.Select(x => new ProveedoresViewModel
                {
                    IdProveedor = x.IdProveedor,
                    Nombre = x.Nombre,
                    Correo = x.Correo,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Direccion = x.Direccion,
                    Fax = x.Fax,
                    Ruc = x.Fax,
                    EstaActivo = x.EstaActivo
                }).Where(x => x.EstaActivo).ToList();
            }
            catch (Exception ex)
                                                                                                                                                                                                                                                                                                                                                                                                             {

                throw;
            }
        }

        public async Task<Response> GuardarProveedor(ProveedoresViewModel proveedoresVM)
        {
            Response resp = new Response();
            try
            {
                var proveedor = new Proveedores
                {
                    Nombre = proveedoresVM.Nombre,
                    Correo =proveedoresVM.Correo,
                    Telefono1 = proveedoresVM.Telefono1,
                    Telefono2 = proveedoresVM.Telefono2,
                    Direccion = proveedoresVM.Direccion,
                    Fax = proveedoresVM.Fax,
                    EstaActivo = true,
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreacion = 1,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioModificacion = 1
                };

                context.Proveedores.Add(proveedor);
                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Guardado con éxito";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar proveedor";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public async Task<Response> ActualizarProveedor(ProveedoresViewModel proveedoresVM)
        {
            Response resp = new Response();
            try
            {
                var proveedor = await context.Proveedores.FirstOrDefaultAsync(x => x.IdProveedor == proveedoresVM.IdProveedor);
                
                proveedor.Nombre = proveedoresVM.Nombre;
                proveedor.Correo = proveedoresVM.Correo;
                proveedor.Telefono1 = proveedoresVM.Telefono1;
                proveedor.Telefono2 = proveedoresVM.Telefono2;
                proveedor.Direccion = proveedoresVM.Direccion;
                proveedor.Fax = proveedoresVM.Fax;
                proveedor.FechaModificacion = DateTime.Now;
                proveedor.IdUsuarioModificacion = 1;
                
                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Actualizado con éxito"; ;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al actualizar proveedor";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public async Task<Response> EliminarProveedor(int idProveedor)
        {
            Response resp = new Response();

            try
            {
                var proveedor = await context.Proveedores.FirstOrDefaultAsync(x => x.IdProveedor == idProveedor);

                if (proveedor == null)
                    throw new Exception("No se encontró proveedor");

                proveedor.EstaActivo = false;
                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Eliminado con éxito"; ;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al eliminar proveedor";
                resp.Error = ex.Message;
                return resp;

            }
        }
        #endregion

        #region Articulo
        public List<ArticuloViewModel> ObtenerArticulos()
        {
            try
            {
                var ArticulosDb =  context.Articulo.Where(x => x.EstaActivo).ToList();

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
                    EstaActivo = x.EstaActivo,
                    Precios = context.Precios.Where(y => y.IdArticulo == x.IdArticulo).Select(
                        z => new PreciosViewModel
                        {
                            PrecioCosto = z.PrecioCosto
                        }).ToList() ?? new List<PreciosViewModel>()
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Pedidos

        public async Task<Response> GuardarPedido(PedidoViewModel pedidoVM)
        {
            Response resp = new Response();
            try
            {
                var pedido = new Pedido
                {
                    IdProveedor = pedidoVM.IdProveedor,
                    NumeroFactura = pedidoVM.NumeroFactura.Value,
                    NumeroReferencia = pedidoVM.NumeroReferencia.Value,
                    Observaciones = pedidoVM.Observaciones, 

                    Subtotal = pedidoVM.DetallePedido.Sum(x => x.Subtotal),
                    Iva = pedidoVM.DetallePedido.Sum(x => x.Iva),
                    Descuento = pedidoVM.DetallePedido.Sum(x => x.Descuento),
                    Total = pedidoVM.DetallePedido.Sum(x => x.Total),

                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioModificacion = 1,
                    IdUsuarioCreacion = 1
                   
                };

                var pedidoDetalle = new List<DetallePedido>();

                foreach (var item in pedidoVM.DetallePedido)
                {
                    pedidoDetalle.Add(new DetallePedido { 
                        IdArticulo = item.Articulo.IdArticulo,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio,
                        Subtotal = item.Subtotal,
                        Descuento = item.Descuento,
                        Iva = item.Iva
                    });
                }

                pedido.DetallePedido = new List<DetallePedido>();
                pedido.DetallePedido = pedidoDetalle;

                context.Pedido.Add(pedido);
                await context.SaveChangesAsync();

                resp.Ok = true;
                resp.Message = "Guardado con éxito";
                return resp;
            }
            catch (Exception  ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar proveedor";
                resp.Error = ex.Message;
                return resp;
            }
        }


        #endregion



    }
}
