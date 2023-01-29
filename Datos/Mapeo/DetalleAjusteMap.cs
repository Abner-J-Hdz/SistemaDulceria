using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class DetalleAjusteMap : IEntityTypeConfiguration<DetalleAjuste>
    {
        public void Configure(EntityTypeBuilder<DetalleAjuste> builder)
        {
            builder.ToTable("TblDetalleAjuste")
                .HasKey(c => c.IdDetalleAjuste);

            builder.HasOne(dt => dt.Ajuste)
                .WithMany(p => p.DetalleAjuste)
                .HasForeignKey(dt => dt.IdAjuste);

            builder.HasOne(dt => dt.Articulo)
                .WithMany()
                .HasForeignKey(dt => dt.IdArticulo);

        }
    }
}
