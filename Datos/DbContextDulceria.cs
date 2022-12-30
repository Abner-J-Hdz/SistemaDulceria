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
            
        }
    }
}
