using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class DetallePedidoMap : IEntityTypeConfiguration<DetallePedido>
    {
        public void Configure(EntityTypeBuilder<DetallePedido> builder)
        {
            builder.ToTable("TblDetallePedido")
                .HasKey(x => x.IdDetallePedido);

            builder.HasOne(dt => dt.Pedido)
                .WithMany(p => p.DetallePedido)
                .HasForeignKey(dt => dt.IdPedido);

            builder.HasOne(dt => dt.Articulo)
                .WithMany()
                .HasForeignKey(dt => dt.IdArticulo);
        }
    }
}
