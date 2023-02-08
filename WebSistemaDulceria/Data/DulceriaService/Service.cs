using Datos;
using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;
using WebSistemaDulceria.Utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
                resp.Message = "Actualizado con éxito";
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
                resp.Message = "Eliminado con éxito";
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
                            IdArticuloPrecio = z.IdArticuloPrecio,
                            PrecioCosto = z.PrecioCosto,
                            PrecioInicial =z.PrecioInicial,
                            MargenGanancia = z.MargenGanancia,
                            PrecioVenta = z.PrecioVenta
                        }).ToList() ?? new List<PreciosViewModel>(),
                    Lote = context.Lote.Where( y => y.IdArticulo == x.IdArticulo).Select(
                        z => new  LoteViewModel
                        {
                            Cantidad = z.Cantidad,
                            IdArticulo = z.IdArticulo,
                            IdLote = z.IdLote,
                        }).ToList() ?? new List<LoteViewModel>(),
                    DetalleProductoTerminado = context.DetalleProductoTerminado.Where(y => y.IdArticuloMaterial == x.IdArticulo)
                    .Select(z => new DetalleProductoTerminadoViewModel { 
                        IdArticuloMaterial = z.IdArticuloMaterial,
                        Cantidad = z.Cantidad,
                        IdArticuloTerminado = z.IdArticuloTerminado
                    }).ToList() ?? new List<DetalleProductoTerminadoViewModel>()
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response> GuardarArticulo(ArticuloViewModel articuloVM)
        {
            Response resp = new Response();
            try
            {
                var articulo = new Articulo
                {
                    Nombre = articuloVM.Nombre,
                    CodBarra = articuloVM.CodBarra,
                    CodInterno = articuloVM.CodInterno,
                    TieneVencimiento = articuloVM.TieneVencimiento,
                    EsMenudeo = articuloVM.EsMenudeo,
                    CantidadMenudeo = articuloVM.CantidadMenudeo,
                    EsProductoTerminado = articuloVM.EsProductoTerminado,
                    IdUnidadMedida = 1,
                    IdPresentacion = 1,
                    EstaActivo = true,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioModificacion = 1,
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreacion = 1,
                    
                };
                articulo.Precios = new List<Precios>();

                articulo.Precios.Add( new Precios {
                        IdArticulo = articuloVM.IdArticulo,
                        PrecioInicial = articuloVM.PrecioInicial,
                        PrecioCosto = articuloVM.PrecioCosto,
                        PrecioVenta = articuloVM.PrecioVenta,
                 });

                context.Articulo.Add(articulo);
                await context.SaveChangesAsync();

                resp.Ok = true;
                resp.Message = "Guardado con éxito";
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

        public async Task<Response> ActualizarArticulo(ArticuloViewModel articuloVM)
        {
            Response resp = new Response();
            try
            {
                var articuloDb = context.Articulo.FirstOrDefault(x => x.IdArticulo == articuloVM.IdArticulo);

                if(articuloDb == null)
                {
                    resp.Ok = false;
                    resp.Message = "No se encontró articulo";
                    return resp;
                }

                articuloDb.Nombre = articuloVM.Nombre;
                articuloDb.CodBarra = articuloVM.CodBarra;
                articuloDb.CodInterno = articuloVM.CodInterno;
                articuloDb.TieneVencimiento = articuloVM.TieneVencimiento;
                articuloDb.EsMenudeo = articuloVM.EsMenudeo;
                articuloDb.CantidadMenudeo = articuloVM.CantidadMenudeo;
                articuloDb.EsProductoTerminado = articuloVM.EsProductoTerminado;
                articuloDb.IdUnidadMedida = 1;
                articuloDb.IdPresentacion = 1;
                articuloDb.FechaModificacion = DateTime.Now;

                var precio = context.Precios.FirstOrDefault(x => x.IdArticuloPrecio == articuloVM.Precios.FirstOrDefault().IdArticuloPrecio);
                precio.PrecioCosto = articuloVM.PrecioCosto;
                precio.PrecioVenta = articuloVM.PrecioVenta;

                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Guardado con éxito";
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

        public async Task<Response> EliminarArticulo(int idArticulo)
        {
            Response resp = new Response();

            try
            {
                var articulo = await context.Articulo.FirstOrDefaultAsync(x => x.IdArticulo == idArticulo);

                if (articulo == null)
                    throw new Exception("No se encontró proveedor");

                articulo.EstaActivo = false;
                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Eliminado con éxito"; ;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al eliminar articulo";
                resp.Error = ex.Message;
                return resp;
            }
        }

        #endregion

        #region Pedidos

        public async Task<Response> GuardarPedido(PedidoViewModel pedidoVM)
        {
            Response resp = new Response();
            try
            {
                var fecha = DateTime.Now;
                var pedido = new Pedido
                {
                    IdProveedor = pedidoVM.Proveedor.IdProveedor,
                    NumeroFactura = pedidoVM.NumeroFactura.Value,
                    NumeroReferencia = pedidoVM.NumeroReferencia.Value,
                    Observaciones = pedidoVM.Observaciones,
                    Fecha = pedidoVM.Fecha,
                    Subtotal = pedidoVM.DetallePedido.Sum(x => x.Subtotal),
                    Iva = pedidoVM.DetallePedido.Sum(x => x.Iva),
                    Descuento = pedidoVM.DetallePedido.Sum(x => x.Descuento),
                    Total = pedidoVM.DetallePedido.Sum(x => x.Total),

                    FechaCreacion = fecha,
                    FechaModificacion = fecha,
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

                foreach (var item in pedidoDetalle)
                {
                    var productoTerminado = context.DetalleProductoTerminado.FirstOrDefault(x => x.IdArticuloMaterial == item.IdArticulo);

                    if(productoTerminado == null)
                    {
                        var productoTerminadoDb = new DetalleProductoTerminado();
                        productoTerminadoDb.IdArticuloMaterial = item.IdArticulo;
                        productoTerminadoDb.Cantidad = (int)item.Cantidad;
                        context.DetalleProductoTerminado.Add(productoTerminadoDb);
                    }
                    else
                    {
                        productoTerminado.Cantidad +=  (int)item.Cantidad;
                    }
                }

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

        public List<PedidoViewModel> ObtenerPedidos()
        {
            List<PedidoViewModel> pedidos = new List<PedidoViewModel>();
            try
            {
                var pedidoDb = context.Pedido.ToList();

                foreach (var item in pedidoDb)
                {
                    pedidos.Add(new PedidoViewModel
                    {
                        IdPedido = item.IdPedido,
                        IdProveedor = item.IdProveedor,
                        Proveedor = new ProveedoresViewModel
                        {
                            IdProveedor = item.IdProveedor,
                            Nombre = context.Proveedores.FirstOrDefault(x => x.IdProveedor == item.IdProveedor)?.Nombre  ?? ""
                        },
                        NumeroFactura= item.NumeroFactura,
                        NumeroReferencia = item.NumeroReferencia,
                        Fecha = item.Fecha
                    });
                }
                return pedidos;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Clientes
        public List<ClientesViewModel> ObtenerClientes()
        {
            try
            {
                var clientesDb = context.Clientes.Where(x => x.EstaActivo).ToList();

                return clientesDb.Select(x => new ClientesViewModel {
                    IdCliente = x.IdCliente,
                    Nombre = x.Nombre,
                    Correo = x.Correo,
                    Telefono1 = x.Telefono1,
                    Telefono2 = x.Telefono2,
                    Direccion = x.Direccion,
                    EstaActivo = x.EstaActivo
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response> GuardarClientes(ClientesViewModel clienteVM)
        {
            Response resp = new Response();
            try
            {
                clienteVM.Telefono1 = clienteVM.Telefono1 == null ? 0 : clienteVM.Telefono1;
                clienteVM.Telefono2 = clienteVM.Telefono2 == null ? 0 : clienteVM.Telefono2;

                var cliente = new Clientes
                {
                    Nombre = clienteVM.Nombre,
                    Correo = clienteVM.Correo,
                    Telefono1 = clienteVM?.Telefono1.Value ?? 0,
                    Telefono2 = clienteVM?.Telefono2.Value ?? 0, 
                    Direccion = clienteVM.Direccion,
                    EstaActivo = true,
                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreacion = 1,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioModificacion = 1
                };

                context.Clientes.Add(cliente);

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

        public async Task<Response> ActualizarClientes(ClientesViewModel clienteVM)
        {
            Response resp = new Response();
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.IdCliente == clienteVM.IdCliente);

                if(cliente == null)
                {
                    resp.Ok = false;
                    resp.Message = "Cliente no encontrado";
                    return resp;
                }

                cliente.Nombre = clienteVM.Nombre;
                cliente.Correo = clienteVM.Correo;
                cliente.Telefono1 = clienteVM?.Telefono1.Value ?? 0;
                cliente.Telefono2 = clienteVM?.Telefono2.Value ?? 0;
                cliente.Direccion = clienteVM.Direccion;
                cliente.FechaModificacion = DateTime.Now;
                cliente.IdUsuarioModificacion = 1;

                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Actualizado con éxito";

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

        public async Task<Response> EliminarClientes(int idCliente)
        {
            Response resp = new Response();
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.IdCliente == idCliente);
                if (cliente == null)
                    throw new Exception("No se encontró proveedor");

                cliente.EstaActivo = false;
                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Eliminado con éxito";
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

        #region Ventas

        public List<VentaViewModel> ObtenerVentasDeDia()
        {
            List<VentaViewModel> ventas = new List<VentaViewModel>();
            try
            {
                var fecha = DateTime.Today;
                var ventasDb = context.Venta.Where(x => x.EstaActiva && x.Fecha.Date == fecha )
                    .OrderByDescending(x => x.IdVenta).ToList();

                foreach (var item in ventasDb)
                {
                    ///obtener el detalle para obtener la cantidad de articulos

                    var detalleVentas = context.DetalleVenta.Where(x => x.IdVenta == item.IdVenta).ToList();

                    ventas.Add(new VentaViewModel
                    {
                        IdVenta = item.IdVenta,
                        IdCliente = item.IdCliente,

                        NumeroRecibo = item.NumeroRecibo,
                        Fecha = item.Fecha,

                        SubTotal = Math.Round(item.SubTotal,2),
                        Descuento = Math.Round(item.Descuento,2),
                        Total = Math.Round(item.Total,2),
                        Iva = Math.Round(item.Iva,2),

                        CantidadArticulos = Math.Round( item.DetalleVenta.Sum(x => x.Cantidad), 2),

                        Cliente = new ClientesViewModel
                        {
                            IdCliente = item.IdCliente,
                            Nombre = context.Clientes.FirstOrDefault(x => x.IdCliente == item.IdCliente)?.Nombre ?? ""
                        },
                    });
                }
                return ventas;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public List<VentaViewModel> ObtenerVentas()
        {
            List<VentaViewModel> ventas = new List<VentaViewModel>();
            try
            {
                var ventasDb = context.Venta.Where(x => x.EstaActiva).OrderByDescending(x => x.IdVenta).ToList();

                foreach (var item in ventasDb)
                {
                    ventas.Add(new VentaViewModel
                    {
                        IdVenta = item.IdVenta,
                        IdCliente = item.IdCliente,
                        
                        NumeroRecibo = item.NumeroRecibo,
                        Fecha = item.Fecha,

                        SubTotal = item.SubTotal,
                        Descuento = item.Descuento,
                        Total = item.Total,
                        Iva = item.Iva,

                        Cliente = new ClientesViewModel
                        {
                            IdCliente = item.IdCliente,
                            Nombre = context.Clientes.FirstOrDefault(x => x.IdCliente == item.IdCliente)?.Nombre ?? ""
                        },
                    });
                }
                return ventas;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<VentaViewModel> ObtenerVenta(int IdVenta)
        {
            try
            {
                var ventaDb = context.Venta.FirstOrDefault(x => x.IdVenta == IdVenta && x.EstaActiva);

                if(ventaDb == null)
                {
                    throw new Exception("NO se encontró venta");
                }

                var ventaVM = new VentaViewModel
                {
                    IdVenta = ventaDb.IdVenta,
                    Fecha = ventaDb.Fecha,
                    IdCliente = ventaDb.IdCliente,
                    NumeroRecibo = ventaDb.NumeroRecibo,
                    SubTotal = Math.Round(ventaDb.SubTotal),
                    Descuento = Math.Round(ventaDb.Descuento),
                    Iva = Math.Round(ventaDb.Iva),
                    Total = Math.Round(ventaDb.Total),
                };
                ventaVM.DetalleVenta = new List<DetalleVentaViewModel>();

                ventaVM.Cliente = new ClientesViewModel();

                var cliente = context.Clientes.FirstOrDefault(x => x.IdCliente == ventaVM.IdCliente);

                if(cliente != null)
                {
                    ventaVM.Cliente = new ClientesViewModel
                    {
                        IdCliente = cliente.IdCliente,
                        Nombre = cliente.Nombre,
                        Correo = cliente.Correo
                    };
                }

                var detalleDb = context.DetalleVenta.Where(x => x.IdVenta == IdVenta).ToList();

                ventaVM.DetalleVenta = detalleDb.Select(x => new DetalleVentaViewModel
                {
                    Editable = true,
                    IdDetalleVenta = x.IdDetalleVenta,
                    IdVenta = x.IdVenta,
                    IdArticulo = x.IdArticulo,
                    Cantidad = Math.Round(x.Cantidad, 2),
                    Precio = Math.Round(x.Precio),
                    SubTotal = Math.Round(x.SubTotal),
                    Descuento = Math.Round( x.Descuento, 2),
                    Iva = Math.Round(x.Iva),
                    Total = Math.Round(((x.Iva + x.SubTotal) - x.Descuento), 2),
                    Articulo = new ArticuloViewModel
                    {
                        IdArticulo = x.IdArticulo,
                        Nombre = context.Articulo.FirstOrDefault(a => a.IdArticulo == x.IdArticulo).Nombre
                    }
                }).ToList();

                return ventaVM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response> EliminarVenta(int IdVenta)
        {
            Response resp = new Response();
            try
            {
                var venta = context.Venta.FirstOrDefault(x => x.IdVenta == IdVenta);

                if (venta == null)
                {
                    resp.Ok = false;
                    resp.Message = "No se encontró venta";
                    return resp;
                }

                var detalleVenta = context.DetalleVenta.Where(x => x.IdVenta == IdVenta).ToList();

                foreach (var item in detalleVenta)
                {
                    var productoTerminado = context.DetalleProductoTerminado.FirstOrDefault(x => x.IdArticuloMaterial == item.IdArticulo);
                    productoTerminado.Cantidad += (int)item.Cantidad;
                }

                venta.EstaActiva = false;

                await context.SaveChangesAsync();

                resp.Ok = true;
                resp.Message = "Venta eliminada correctamente";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al eliminar";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public async Task<Response> GuardarVentas(VentaViewModel ventaVM)
        {
            Response resp = new Response();

            try
            {
                int NumeroRecibo = context.Venta.OrderByDescending(x => x.IdVenta).FirstOrDefault()?.IdVenta + 1 ?? 1;
                var venta = new Venta
                {
                    IdCliente = ventaVM.Cliente.IdCliente,
                    Fecha = ventaVM.Fecha,
                    NumeroRecibo = NumeroRecibo,
                    SubTotal = ventaVM.DetalleVenta.Sum(x => x.SubTotal),
                    Descuento = ventaVM.DetalleVenta.Sum(x => x.Descuento),
                    Iva = ventaVM.DetalleVenta.Sum(x => x.Iva),
                    Total = ventaVM.DetalleVenta.Sum(x => x.Total),
                    EstaActiva = true,
                    Observaciones = ventaVM.Observaciones,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioCreacion = 1
                };

                var detalleVenta = new List<DetalleVenta>();

                foreach (var item in ventaVM.DetalleVenta)
                {
                    detalleVenta.Add(new DetalleVenta
                    {
                        IdArticulo = item.Articulo.IdArticulo,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio, 
                        SubTotal = item.SubTotal,
                        Descuento = item.Descuento,
                        Iva = item.Iva,
                        PocentajeDescuento = item.ValorDescuento
                    });
                }

                foreach (var item in detalleVenta)
                {
                    var productoTerminado = context.DetalleProductoTerminado.FirstOrDefault(x => x.IdArticuloMaterial == item.IdArticulo);
                    string NombreArticulo = context.Articulo.FirstOrDefault(x => x.IdArticulo == item.IdArticulo)?.Nombre ?? "";

                    if (productoTerminado == null)
                    {
                        resp.Message = "Articulo " + NombreArticulo + " sin cantidad.";
                        resp.Ok = false;
                        return resp;
                    }
                    else
                    {
                        var restaProduct = productoTerminado.Cantidad - (int)item.Cantidad;

                        if(restaProduct < 0)
                        {
                            resp.Message = "Articulo " + NombreArticulo + "sin cantidad suficiente.";
                            resp.Ok = false;
                            return resp;
                        }

                        productoTerminado.Cantidad -= (int)item.Cantidad;
                    }
                }

                venta.DetalleVenta = detalleVenta;
                context.Venta.Add(venta);
                await context.SaveChangesAsync();

                resp.Ok = true;
                resp.Message = "Guardado con éxito";
                resp.ResponseParameter1 = venta.IdVenta.ToString();
                resp.ResponseParameter2 = NumeroRecibo.ToString();
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar venta";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public VentaViewModel ObtenerUltimaVenta()
        {
            try
            {
                var ventaDb = context.Venta.OrderByDescending(x => x.IdVenta).FirstOrDefault();

                if (ventaDb == null)
                {
                    throw new Exception("NO se encontró venta");
                }

                var ventaVM = new VentaViewModel
                {
                    IdVenta = ventaDb.IdVenta,
                    Fecha = ventaDb.Fecha,
                    NumeroRecibo = ventaDb.NumeroRecibo,
                    SubTotal = ventaDb.SubTotal,
                    Descuento = ventaDb.Descuento,
                    Iva = ventaDb.Iva,
                    Total = ventaDb.Total,
                };
                ventaVM.DetalleVenta = new List<DetalleVentaViewModel>();

                ventaVM.Cliente = new ClientesViewModel();

                var cliente = context.Clientes.FirstOrDefault(x => x.IdCliente == ventaVM.IdCliente);

                if (cliente != null)
                {
                    ventaVM.Cliente = new ClientesViewModel
                    {
                        IdCliente = cliente.IdCliente,
                        Nombre = cliente.Nombre
                    };
                }

                var detalleDb = context.DetalleVenta.Where(x => x.IdVenta == ventaDb.IdVenta).ToList();

                ventaVM.DetalleVenta = detalleDb.Select(x => new DetalleVentaViewModel
                {
                    Editable = true,
                    IdDetalleVenta = x.IdDetalleVenta,
                    IdVenta = x.IdVenta,
                    IdArticulo = x.IdArticulo,
                    Cantidad = x.Cantidad,
                    Precio = x.Precio,
                    SubTotal = x.SubTotal,
                    Descuento = x.Descuento,
                    Iva = x.Iva,
                    Total = (x.Iva + x.SubTotal) - x.Descuento,
                    Articulo = new ArticuloViewModel
                    {
                        IdArticulo = x.Articulo.IdArticulo,
                        Nombre = x.Articulo.Nombre
                    }
                }).ToList();

                return ventaVM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Ajustes de Inventarios

        public async Task<Response> GuardarAjusteInventario(AjusteViewModel ajuste)
        {
            Response resp = new Response();
            try
            {
                int numeroReferencia = context.Ajuste.OrderByDescending(x => x.IdAjuste).FirstOrDefault()?.NumeroRefencia + 1 ?? 1;

                DateTime fechaHoy = DateTime.Now;

                var ajusteDb = new Ajuste { 
                    NumeroRefencia = numeroReferencia,
                    Observaciones = ajuste.Observaciones,
                    EsAjusteEntrada = ajuste.EsAjusteEntrada,
                    Total = ajuste.Total,
                    FechaCreacion = ajuste.FechaCreacion,
                    EstaActivo = true,
                    IdUsuarioCreacion = 1,
                    FechaModificacion = ajuste.FechaCreacion,
                    IdUsuarioModficacion = 1
                };

                var detalleAjuste = new List<DetalleAjuste>();

                foreach (var item in ajuste.DetalleAjuste)
                {
                    detalleAjuste.Add(new DetalleAjuste
                    {
                        IdArticuloMaterial = item.IdArticulo,
                        FechaVencimiento = fechaHoy,//item.FechaVencimiento,
                        Cantidad = item.Cantidad,
                        Costo = item.Costo,
                        Total  = item.TotalDetalleAjuste,
                        FechaCreacion = fechaHoy,
                        FechaModificacion = fechaHoy,
                        IdUsuarioCreacion = 1,
                        IdUsuarioModficacion = 1
                    });
                }

                //codigo para aumentar el stock teniendo en cuanta si es un ajuste entrada
                /*aumentar el stock si ajuste entrada es true*/
                foreach (var item in detalleAjuste)
                {
                    var productoTerminado = context.DetalleProductoTerminado.FirstOrDefault(x => x.IdArticuloMaterial == item.IdArticuloMaterial);
                    string NombreArticulo = context.Articulo.FirstOrDefault(x => x.IdArticulo == item.IdArticuloMaterial)?.Nombre ?? "";

                    if (ajuste.EsAjusteEntrada)
                    {
                        if (productoTerminado == null)
                        {
                            var detelleProducto = new DetalleProductoTerminado();
                            detelleProducto.IdArticuloMaterial = item.IdArticuloMaterial;
                            detelleProducto.Cantidad = (int)item.Cantidad;
                            context.DetalleProductoTerminado.Add(detelleProducto);
                        }
                        else
                        {
                            productoTerminado.Cantidad += (int)item.Cantidad;
                        }
                    }
                    else
                    {
                        var restaProduct = productoTerminado.Cantidad - (int)item.Cantidad;

                        if (restaProduct < 0)
                        {
                            resp.Message = "Articulo " + NombreArticulo + "sin cantidad suficiente.";
                            resp.Ok = false;
                            return resp;
                        }

                        productoTerminado.Cantidad -= (int)item.Cantidad;
                    }

                }

                ajusteDb.DetalleAjuste = detalleAjuste;
                context.Ajuste.Add(ajusteDb);

                await context.SaveChangesAsync();

                resp.Ok = true;
                resp.Message = "Guardado con éxito";
                resp.ResponseParameter1 = ajuste.IdAjuste.ToString();
                resp.ResponseParameter2 = numeroReferencia.ToString();

                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar el ajuste de inventario";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public List<AjusteViewModel> ObtenerAjustesinventario()
        {
            List<AjusteViewModel> ajuste = new List<AjusteViewModel>();
            try
            {
                var ajusteDb = context.Ajuste.Where(x => x.EstaActivo).ToList();

                foreach (var item in ajusteDb)
                {
                    ajuste.Add(new AjusteViewModel
                    {
                        IdAjuste = item.IdAjuste,
                        IdUsuarioCreacion = item.IdUsuarioCreacion,
                        NumeroRefencia = item.NumeroRefencia,
                        FechaCreacion = item.FechaCreacion,
                        Total = item.Total,
                        AjsuteEntrada = item.EsAjusteEntrada ? "Si" : "No",
                    Observaciones = item.Observaciones,
                        usuario = new UsuarioViewModel
                        {
                            IdUsuario = item.IdUsuarioCreacion,
                            Nombre = context.Usuarios.FirstOrDefault(x => x.IdUsuario == item.IdUsuarioCreacion)?.Nombre ?? ""
                        },
                    });
                }

                return ajuste;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AjusteViewModel ObtenerUnAjusteInventario(int Id)
        {
            try
            {
                AjusteViewModel ajuste = new AjusteViewModel();

                var ajusteDb = context.Ajuste.FirstOrDefault(x => x.IdAjuste == Id);

                if (ajusteDb == null)
                    throw new Exception("Ajuste de inventario no encontrado");

                ajuste.IdAjuste = ajusteDb.IdAjuste;
                ajuste.Observaciones = ajusteDb.Observaciones;
                ajuste.IdUsuarioCreacion = ajusteDb.IdUsuarioCreacion;
                ajuste.NumeroRefencia = ajusteDb.NumeroRefencia;
                ajuste.EsAjusteEntrada = ajusteDb.EsAjusteEntrada;
                ajuste.AjsuteEntrada = ajusteDb.EsAjusteEntrada ? "Si" : "No";
                ajuste.FechaCreacion = ajusteDb.FechaCreacion;
                ajuste.Total = ajusteDb.Total;
                ajuste.usuario = new UsuarioViewModel
                {
                    IdUsuario = ajusteDb.IdUsuarioCreacion,
                    Nombre = context.Usuarios.FirstOrDefault(x => x.IdUsuario == ajusteDb.IdUsuarioCreacion)?.Nombre ?? ""
                };

                var ajusteDetalle = context.DetalleAjuste.Where(x => x.IdAjuste == Id).ToList();
                ajuste.DetalleAjuste = ajusteDetalle.Select(x => new DetalleAjusteViewModel
                {
                    Editable = true,
                    IdDetalleAjuste = x.IdDetalleAjuste,
                    IdAjuste = x.IdAjuste,
                    IdArticulo = x.IdArticuloMaterial,
                    Costo = x.Costo,
                    Cantidad = x.Cantidad,
                    TotalDetalleAjuste = x.Cantidad * x.Costo,
                    Articulo = new ArticuloViewModel { 
                        IdArticulo = x.IdAjuste,
                        Nombre = context.Articulo.FirstOrDefault(y=>y.IdArticulo == x.IdArticuloMaterial)?.Nombre ?? ""
                    }
                }).ToList();

                return ajuste;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response> EliminarAjusteInventario(int IdAjuste) 
        {
            Response resp = new Response();
            try
            {
                var ajusteDb = context.Ajuste.FirstOrDefault(x => x.IdAjuste ==  IdAjuste);

                if (ajusteDb == null)
                {
                    resp.Ok = false;
                    resp.Message = "No se encontró ajuste";
                    return resp;
                }

                var detalleAjuste = context.DetalleAjuste.Where(x => x.IdAjuste == IdAjuste).ToList();

                foreach (var item in detalleAjuste)
                {
                    var productoTerminado = context.DetalleProductoTerminado.FirstOrDefault(x => x.IdArticuloMaterial == item.IdArticuloMaterial);
                    int CantidaRestante = 0;
                    if (ajusteDb.EsAjusteEntrada)
                        CantidaRestante = productoTerminado.Cantidad - (int)item.Cantidad;
                    else
                        productoTerminado.Cantidad += (int)item.Cantidad;

                    if (CantidaRestante < 0)
                    {
                        resp.Ok = true;
                        resp.Message = $"El articulo ${productoTerminado.Articulo.Nombre} no puede quedar en un numero menor a 0";
                        return resp;
                    }
                }

                ajusteDb.EstaActivo = false;

                await context.SaveChangesAsync();

                resp.Ok = true;
                resp.Message = "Ajuste eliminado correctamente";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al eliminar";
                resp.Error = ex.Message;
                return resp;
            }
        }


        #endregion

        #region Usuarios

        public async Task<Response> GuardarUsuario(UsuarioViewModel usuarioVM)
        {
            Response resp = new Response();
            try
            {
                Usuarios usuario = new Usuarios
                {
                    Nombre = usuarioVM.Nombre,
                    Apellido = usuarioVM.Apellido,
                    Email = usuarioVM.Email,
                    EstaActivo = true,
                    Password = "z8jTNljQU9Y="
                };

                context.Usuarios.Add(usuario);

                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Usuario creado correctamente";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar usuario";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public List<UsuarioViewModel> ObtenerUsuario() {
            try
            {
                var usuariosDb = context.Usuarios.Where(x => x.EstaActivo == true).ToList();

                return usuariosDb.Select(x => new UsuarioViewModel
                {
                    IdUsuario = x.IdUsuario,
                    Nombre = x.Nombre,
                    Apellido = x.Apellido,
                    Email = x.Email,
                }).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Response> ActualizarUsuario(UsuarioViewModel usuarioVM)
        {
            Response resp = new Response();
            try
            {
                var UsuarioDb = context.Usuarios.FirstOrDefault(x => x.IdUsuario == usuarioVM.IdUsuario);

                if (UsuarioDb == null)
                {
                    resp.Ok = false;
                    resp.Message = "Usuario no encontrado";
                    return resp;
                }

                UsuarioDb.Nombre = usuarioVM.Nombre;
                UsuarioDb.Apellido = usuarioVM.Apellido;
                UsuarioDb.Email = usuarioVM.Email;

                await context.SaveChangesAsync();

                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al actualizar usuario";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public async Task<Response> ActualizarPassword(int IdUsuario, string password)
        {
            Response resp = new Response();
            try
            {
                var UsuarioDb = context.Usuarios.FirstOrDefault(x => x.IdUsuario == IdUsuario);

                if (UsuarioDb == null)
                {
                    resp.Ok = false;
                    resp.Message = "Usuario no encontrado";
                    return resp;
                }

                UsuarioDb.Nombre = password;

                await context.SaveChangesAsync();

                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al actualizar usuario";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public async Task<Response> EliminarUsuario(int IdUsuario)
        {
            Response resp = new Response();
            try
            {
                var UsuarioDb = context.Usuarios.FirstOrDefault(x => x.IdUsuario == IdUsuario);

                if (UsuarioDb == null)
                {
                    resp.Ok = false;
                    resp.Message = "Usuario no encontrado";
                    return resp;
                }

                UsuarioDb.EstaActivo = false;

                await context.SaveChangesAsync();
                resp.Message = "Usuario eliminado correctamente";
                resp.Ok = true;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al eliminar usuario";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public Response GetUsuarioLogin(string email, string password)
        {
            Response resp = new Response();

            EncryptMd5 encrypt = new EncryptMd5();

            try
            {
                var usuario = context.Usuarios.FirstOrDefault(x => x.Email == email);

                usuario.Password = encrypt.Encrypt(password);

                if(usuario == null)
                {
                    resp.Ok = false;
                    resp.Error = "usuario no encontrado";

                    return resp;
                }

                if(usuario.Password == password)
                {
                    resp.Ok = true;
                    resp.ResponseParameter1 = usuario.IdUsuario.ToString();
                    return resp;
                }

                resp.Ok = false;
                resp.Error = "Contraseña invalida";
                return resp;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Response> CambiarContraseña(string email, string password)
        {
            Response resp = new Response();
            EncryptMd5 encrypt = new EncryptMd5();

            try
            {
                var usuario = context.Usuarios.FirstOrDefault(x => x.Email == email);

                if(usuario == null)
                {
                    resp.Ok = false;
                    resp.Message = "Ocurrió un error al cambiar contraseña";
                    resp.Error = "Usuario no encontrado";
                    return resp;
                }

                usuario.Password = encrypt.Encrypt(password);

                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Se actualizó contraseña";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ocurrió un error al cambiar contraseña";
                resp.Error = ex.Message;
                return resp;
            }
        }

        #endregion

        #region TblObjecto

        public async Task<Response> GuardarObjecto(ObjectoViewModel objectoVM)
        {
            Response resp = new Response();
            try
            {
                Objecto objectoDb = new Objecto
                {
                    CodigoInterno = objectoVM.CodigoInterno,
                    Nombre = objectoVM.Nombre,
                    IdTipoObjeto = objectoVM.IdTipoObjeto,
                    EstaActivo = true,

                    FechaCreacion = DateTime.Now,
                    IdUsuarioCreacion = 1,
                    FechaModificacion = DateTime.Now,
                    IdUsuarioModificacion = 1                    
                };

                context.Objecto.Add(objectoDb);

                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Objecto creado correctamente";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar objecto";
                resp.Error = ex.Message;
                return resp;
            }
        }

        public async Task<Response> ActualizarObjeto(ObjectoViewModel objectoVM)
        {
            Response resp = new Response();
            try
            {
                var objetoDb = context.Objecto.FirstOrDefault(x => x.IdObjeto == objectoVM.IdObjeto);

                if (objetoDb  == null)
                {
                    resp.Ok = true;
                    resp.Message = "No se encontró objeto";
                    return resp;
                }

                objetoDb.CodigoInterno = objectoVM.CodigoInterno;
                objetoDb.Nombre = objectoVM.Nombre;
                objetoDb.IdTipoObjeto = objectoVM.IdTipoObjeto;
                objetoDb.EstaActivo = true;
                objetoDb.FechaModificacion = DateTime.Now;
                objetoDb.IdUsuarioModificacion = 1;

                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Objecto creado correctamente";
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al guardar objecto";
                resp.Error = ex.Message;
                return resp;
            }
        }


        public List<TipoObjectoViewModel> ObtenerTipoObjecto()
        {
            try
            {
                return context.TipoObjecto.Where(x => x.EstaActivo).Select(x => new TipoObjectoViewModel
                {
                    CodigoInterno = x.CodigoInterno,
                    IdTipoObjeto = x.IdTipoObjeto,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                }).ToList();
                //return new List<TipoObjectoViewModel>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ObjectoViewModel> ObtenerObjecto()
        {
            try
            {
                return context.Objecto.Where(x => x.EstaActivo).Select(x => new ObjectoViewModel
                {
                    IdObjeto = x.IdObjeto,
                    CodigoInterno = x.CodigoInterno,
                    IdTipoObjeto = x.IdTipoObjeto,
                    Nombre = x.Nombre,
                    TipoObjecto = new TipoObjectoViewModel
                    {
                        IdTipoObjeto = x.IdTipoObjeto,
                        Nombre = context.TipoObjecto.FirstOrDefault(t => t.IdTipoObjeto == x.IdTipoObjeto).Nombre ?? "",
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Response> EliminarObjeto(int idObjecto)
        {
            Response resp = new Response();

            try
            {
                var objeto = await context.Objecto.FirstOrDefaultAsync(x => x.IdObjeto == idObjecto);

                if (objeto == null)
                    throw new Exception("No se encontró objeto");

                objeto.EstaActivo = false;
                await context.SaveChangesAsync();
                resp.Ok = true;
                resp.Message = "Eliminado con éxito"; ;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Ok = false;
                resp.Message = "Ha ocurrido un error al eliminar objeto";
                resp.Error = ex.Message;
                return resp;
            }
        }


        #endregion
    }
}

/*
Agrega el atributo Authorize a las páginas o componentes que requieren autenticación para protegerlos de acceso no autorizado:
Copy code
@attribute [Authorize]*/
