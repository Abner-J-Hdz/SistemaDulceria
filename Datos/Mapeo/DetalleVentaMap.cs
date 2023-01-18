using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class DetalleVentaMap : IEntityTypeConfiguration<DetalleVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleVenta> builder)
        {
            builder.ToTable("TblDetalleVenta")
                .HasKey(x => x.IdDetalleVenta);

            builder.HasOne(dt => dt.Venta)
                .WithMany(p => p.DetalleVenta)
                .HasForeignKey(dt => dt.IdVenta);

            builder.HasOne(dt => dt.Articulo)
                .WithMany()
                .HasForeignKey(dt => dt.IdArticulo);
        }
    }
}
