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

        public List<VentaViewModel> ObtenerVentas()
        {
            List<VentaViewModel> ventas = new List<VentaViewModel>();
            try
            {
                var ventasDb = context.Venta.ToList();

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
                    SubTotal = ventaVM.SubTotal,
                    Descuento = ventaVM.Descuento,
                    Iva = ventaVM.Iva,
                    Total = ventaVM.Total,
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
                        Iva = item.Iva
                    });
                }

                foreach (var item in detalleVenta)
                {
                    var productoTerminado = context.DetalleProductoTerminado.FirstOrDefault(x => x.IdArticuloMaterial == item.IdArticulo);
                    string NombreArticulo = context.Articulo.FirstOrDefault(x => x.IdArticulo == item.IdArticulo)?.Nombre ?? "";

                    if (productoTerminado == null)
                    {
                        resp.Message = "Articulo " + NombreArticulo + "sin cantidad.";
                    }
                    else
                    {
                        var restaProduct = productoTerminado.Cantidad - (int)item.Cantidad;

                        if(restaProduct < 0)
                        {
                            resp.Message = "Articulo " + NombreArticulo + "sin cantidad suficiente.";
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
                    Password = ""
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
                resp.Message = "Ha ocurrido un error al eliminar usuario";
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

        #endregion


    }
}

/*
Agrega el atributo Authorize a las páginas o componentes que requieren autenticación para protegerlos de acceso no autorizado:
Copy code
@attribute [Authorize]*/
