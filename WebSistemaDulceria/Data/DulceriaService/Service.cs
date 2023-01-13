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

        public Response GetUsuarioLogin(string email, string password)
        {
            Response resp = new Response();

            try
            {
                var usuario = context.Usuarios.FirstOrDefault(x => x.Email == email);

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

        /*public async Task onLogin(string email, string password)
        {
            //validar el usuario en la base de datos
            if (email == "admin@dulceria.com" && password == "admin123")
            {
                // Iniciar una sesión
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "vendedor user"),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

  

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }
        }*/


        //public Task OnLogout()
        //{
        /*await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignOutAsync();*/
        //}
    }
}

/*
Agrega el atributo Authorize a las páginas o componentes que requieren autenticación para protegerlos de acceso no autorizado:
Copy code
@attribute [Authorize]*/
