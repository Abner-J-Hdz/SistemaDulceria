using Datos.Mapeo;
using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class DbContextDulceria: DbContext
    {
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<DetallePedido> DetallePedido { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Precios> Precios { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Lote> Lote { get; set; }
        public DbSet<DetalleProductoTerminado> DetalleProductoTerminado { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<Ajuste> Ajuste { get; set; }
        public DbSet<DetalleAjuste> DetalleAjuste { get; set; }
        public DbSet<TipoObjecto> TipoObjecto { get; set; }
        public DbSet<Objecto> Objecto { get; set; }

        public DbContextDulceria(DbContextOptions<DbContextDulceria> options) : base(options)
        {

        }

        protected   override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProveedoresMap());
            modelBuilder.ApplyConfiguration(new PedidoMap());
            modelBuilder.ApplyConfiguration(new DetallePedidoMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new PreciosMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new LoteMap());
            modelBuilder.ApplyConfiguration(new DetalleProductoTerminadoMap());
            modelBuilder.ApplyConfiguration(new ClientesMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());
            modelBuilder.ApplyConfiguration(new AjusteMap());
            modelBuilder.ApplyConfiguration(new DetalleAjusteMap());
            modelBuilder.ApplyConfiguration(new ObjectoMap());
            modelBuilder.ApplyConfiguration(new TipoObjetoMap());
        }
    }
}
